using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PRN211_test.Enums;
using PRN211_project.Models;

namespace PRN211_project.Fillters
{
    public class AdminFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Access HttpContext through the context parameter
            var httpContext = context.HttpContext;

            var sesUserJson = httpContext.Session.GetString("sesUser");

            Account sesUser = JsonConvert.DeserializeObject<Account>(sesUserJson);

            if (sesUser.Roll != (int)RollType.Admin)
            {
                context.Result = new RedirectToActionResult("error", "home", null);
                return;
            }
        }
    }
}
