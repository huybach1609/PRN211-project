using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json;
using PRN211_test.Enums;
using PRN211_project.Models;

namespace PRN211_project.Fillters
{
    public class AuthFilter: IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var sesUserJson = httpContext.Session.GetString("sesUser");
            if (sesUserJson != null)
            {
                context.Result = new RedirectToActionResult("", "home", null);
                return;
            }
        }
    }
}
