using todoapp.server.Constants;

namespace todoapp.server.Extensions
{
    public static class WebApplicationExtension
    {
        public static IApplicationBuilder UseAppPipeline(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
