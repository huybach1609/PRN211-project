using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using PRN211_project.Models;

namespace PRN211_project.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public string checkStatus(int id)
        {
           Models.Task result =  PRN211_projectContext.Ins.Tasks.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                result.Status = !result.Status;
                PRN211_projectContext.Ins.SaveChanges();
            }

            return "";
        }
    }
}
