using Microsoft.Extensions.FileSystemGlobbing.Internal;
using PRN211_project.Fillters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();


//config path
//app.MapGet("/", () => "");

// product/list
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}"
    );
// product/delete/1
app.MapControllerRoute(
    name: "default2",
    pattern: "{controller}/{action}/{id}"
    );

app.Run();
