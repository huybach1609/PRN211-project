using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using todoapp.server.Constants;
using todoapp.server.Dtos;
using todoapp.server.Models;
using todoapp.server.Services.Implementations;
using todoapp.server.Services.Interfaces;
using todoapp.server.Services.Jwt;
using todoapp.server.Services.Mail;

namespace todoapp.server.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAppCore(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            // Build OData model once here (avoid re-adding controllers later)
            //var odataModel = BuildODataModel();
            services.AddControllers();

            //services
            //    .AddControllers()
            //    .AddOData(opt => opt
            //        .Select()
            //        .Filter()
            //        .Count()
            //        .OrderBy()
            //        .Expand()
            //        .SetMaxTop(ODataEntitySets.MaxTop)
            //        .AddRouteComponents(ODataEntitySets.RoutePrefix, odataModel));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        Field = e.Key,
                        Messages = e.Value.Errors.Select(er => er.ErrorMessage)
                    });
                    return new BadRequestObjectResult(new
                    {
                        Message = "Validation errors occurred.",
                        Errors = errors
                    });
                };
            });


            // Session
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(30);
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });


            return services;
        }

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration config)
        {
            var origins = config.GetSection(ConfigurationConstants.AllowedCorsOrigins)
                                .Get<string[]>() ?? Array.Empty<string>();

            foreach (var i in origins)
            {
                Console.WriteLine(i);
            }


            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.ReactApp, b =>
                {
                    //b.WithOrigins(origins)
                    b.AllowAnyOrigin()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                    //.AllowCredentials();
                });
            });

            return services;
        }

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            // DbContext
            var connKey = environment.IsDevelopment()
                ? ConfigurationConstants.DefaultConnectionString
                : ConfigurationConstants.RemoteConnectionString;

            services.AddDbContext<Prn231ProjectContext>(options =>
                options.UseSqlServer(configuration[connKey]));


            // add mapper config
            IMapper mapper = new MapperConfiguration(cfg =>
                    { cfg.AddProfile(new MapperConfig()); })
                .CreateMapper();
            services.AddSingleton(mapper);

            // Services (Scoped for EF usage)
            services.AddScoped<ObjectMapper>();
            services.AddScoped<MailService>();

            services.AddScoped<TagService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IListService, ListService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStickyNoteService, StickyNoteService>();

            services.AddSingleton<IJwtService, JwtService>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            var secret = configuration[ConfigurationConstants.SecretKeyJwtSettings];
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException(
                    $"Missing or empty JWT secret at '{ConfigurationConstants.SecretKeyJwtSettings}'.");


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };

                        if (environment.IsDevelopment())
                        {
                            options.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    var bearer = context.Request.Headers["Authorization"].ToString();
                                    if (bearer.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                    {
                                        context.Token = bearer.Substring("Bearer ".Length).Trim();
                                    }
                                    return System.Threading.Tasks.Task.CompletedTask;
                                }
                            };
                        }
                    });

            services.AddAuthorizationBuilder()
                    .AddPolicy("AdminOnly", policy =>
                        policy.RequireRole(UserRole.Admin.ToString()))
                    .AddPolicy("UserOnly", policy =>
                        policy.RequireRole(UserRole.User.ToString()))
                    .AddPolicy("UserOrAdmin", policy =>
                        policy.RequireRole(UserRole.User.ToString(), UserRole.Admin.ToString()));

            return services;
        }

        //// Build EDM once
        //private static Microsoft.OData.Edm.IEdmModel BuildODataModel()
        //{
        //    var modelBuilder = new ODataConventionModelBuilder();
        //    modelBuilder.EntitySet<List>("Lists");
        //    modelBuilder.EntitySet<todoapp.server.Models.Task>("Tasks");
        //    return modelBuilder.GetEdmModel();
        //}
    }
}
