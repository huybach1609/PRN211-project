using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using PRN211_test.Enums;
using PRN211_project.Models;

namespace PRN211_project.Fillters
{
    public class AuthenticationFillter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Access HttpContext through the context parameter
            var httpContext = context.HttpContext;

            var sesUserJson = httpContext.Session.GetString("sesUser");

            if (sesUserJson == null)
            {
                context.Result = new RedirectToActionResult("login", "auth", null);
                return;
            }
        }
    }
}
