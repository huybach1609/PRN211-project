using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;
using todoapp.server.Constants;
using todoapp.server.Dtos;
using todoapp.server.Exceptions;
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
            var odataModel = BuildODataModel();

            services
                .AddControllers()
                .AddOData(opt => opt
                    .Select()
                    .Filter()
                    .Count()
                    .OrderBy()
                    .Expand()
                    .SetMaxTop(ODataEntitySets.MaxTop)
                    .AddRouteComponents(ODataEntitySets.RoutePrefix, odataModel));

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //services.AddSwaggerGen();


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

            services.AddAuthorization();
            return services;
        }

        // Build EDM once
        private static Microsoft.OData.Edm.IEdmModel BuildODataModel()
        {
            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<List>("Lists");
            modelBuilder.EntitySet<todoapp.server.Models.Task>("Tasks");
            return modelBuilder.GetEdmModel();
        }
    }
}
