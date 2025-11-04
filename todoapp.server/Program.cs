using todoapp.server.Extensions;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);



// Services
builder.Services
    .AddAppCore(builder.Configuration)
    .AddCorsPolicies(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration, builder.Environment)
    .AddApplicationServices(builder.Configuration, builder.Environment);

// services

var app = builder.Build();



// Pipeline
app.UseAppPipeline(app.Environment);
app.MapControllers();

app.Run();
