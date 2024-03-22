using Microsoft.AspNetCore.Mvc;
using PRN211_project.Fillters;

namespace PRN211_project.Controllers
{
    [TypeFilter(typeof(AuthenticationFillter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult Error()
        {
            ViewData["Title"] = "Permission denied";
            ViewData["mess"] = "You do not have permission to acsses this page";
            return View();
        }

    }
}
