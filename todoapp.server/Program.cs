using Microsoft.EntityFrameworkCore;
using todoapp.server.Models;
using todoapp.server.Services.Jwt;
using todoapp.server.Services.Mail;
using todoapp.server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using todoapp.server.Services.iml;
using Microsoft.AspNetCore.OData;
using System.Reflection.Emit;
using Microsoft.OData.ModelBuilder;
using AutoMapper;
using todoapp.server;
using todoapp.server.Controllers;
using todoapp.server.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



// authentication
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


// obdata config
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<List>("Lists");
modelBuilder.EntitySet<todoapp.server.Models.Task>("Tasks");

// default
builder.Services.AddControllers()
   .AddOData(opt => opt
        .Select()
        .Filter()
        .Count()
        .OrderBy()
        .Expand()
        .SetMaxTop(100)
        .AddRouteComponents("odata", modelBuilder.GetEdmModel()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add mapper config + di
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapperConfig());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// add httpcontext service
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();


// set up di database
builder.Services.AddDbContext<Prn231ProjectContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// add custom services to builder
builder.Services.AddScoped<ObjectMapper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MailService>();
builder.Services.AddTransient<TagService>();
builder.Services.AddTransient<IListService, ListService>();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IStickyNoteService, StickyNoteService>();
builder.Services.AddSingleton<IJwtService, JwtServiceImp>();

// set up session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// set up authentication by jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].ToString();

            Console.WriteLine($"Raw Authorization Header: '{token}'");

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var jwt = token.Substring("Bearer ".Length).Trim();
                Console.WriteLine($"Extracted Token: '{jwt}'");
            }
            return System.Threading.Tasks.Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Secret Key: {builder.Configuration["JwtSettings:SecretKey"]}");
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return System.Threading.Tasks.Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Token validated: {context.Principal.Identity.Name}");
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };

});

// Add CORS configuration to the service collection
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Include if you need cookies/auth
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("ReactAppPolicy");// cors

app.UseAuthentication(); // use authentications
app.UseAuthorization();

app.UseSession();


app.MapControllers();

app.Run();
