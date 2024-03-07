using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Newtonsoft.Json;
using NuGet.Protocol;
using PRN211_project.Fillters;
using PRN211_project.Models;

namespace PRN211_project.Controllers
{

    [TypeFilter(typeof(AuthenticationFillter))]
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show()
        {
            ViewData["Title"] = "Show All Task";
            ViewData["Head"] = "All Task";
            Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));
            List<Models.Task> tasks = PRN211_projectContext.Ins.Tasks.Where(x => x.List.AccountId == acc.Id).ToList();
            List<Models.List> lists = PRN211_projectContext.Ins.Lists.Where(x => x.AccountId == acc.Id).ToList();
            ViewBag.Tasks = tasks;
            ViewBag.Lists = lists;
            return View();
        }
        [HttpPost]
        public string insert(Models.Task input)
        {
            PRN211_projectContext.Ins.Tasks.Add(input);
            PRN211_projectContext.Ins.SaveChanges();

            return input.ToString();
        }

        [HttpPost]
        public string GetData(int taskId)
        {
            Models.Task result = PRN211_projectContext.Ins.Tasks.FirstOrDefault(x => x.Id == taskId);

            string jsonString = "";
            if (result != null)
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                jsonString = JsonConvert.SerializeObject(result, settings);
            }
            return jsonString;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Models.Task result = PRN211_projectContext.Ins.Tasks.FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                PRN211_projectContext.Ins.Remove(result);
                PRN211_projectContext.Ins.SaveChanges();
            }
            return Redirect("/task/show");
        }

        [HttpGet]
        public string CheckStatus(int id)
        {
            Models.Task result = PRN211_projectContext.Ins.Tasks.FirstOrDefault(x => x.Id == id);
            if (result != null)
            {
                result.Status = !result.Status;
                PRN211_projectContext.Ins.SaveChanges();
            }
            return "";
        }
    }

}
