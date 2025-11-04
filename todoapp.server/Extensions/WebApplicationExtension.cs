using todoapp.server.Constants;
using Scalar.AspNetCore;

namespace todoapp.server.Extensions
{
    public static class WebApplicationExtension
    {
        public static IApplicationBuilder UseAppPipeline(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseSwagger();

                app.MapSwagger("/openapi/{documentName}.json");
                app.MapScalarApiReference(options => {
                    options.WithTitle("TodoApp API document")
                    .HideClientButton()
                    .ExpandAllTags()
                    .WithTheme(ScalarTheme.Mars);
                });
            }


            app.UseHttpsRedirection();
            app.UseCors(CorsPolicies.ReactApp);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            return app;
        }
    }
}
