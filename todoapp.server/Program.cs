using todoapp.server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services
    .AddAppCore(builder.Configuration)
    .AddCorsPolicies(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration, builder.Environment)
    .AddApplicationServices(builder.Configuration, builder.Environment);

var app = builder.Build();

// Pipeline
app.UseAppPipeline(app.Environment);
app.MapControllers();

app.Run();
