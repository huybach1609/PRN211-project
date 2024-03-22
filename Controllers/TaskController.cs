using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                ViewData["Title"] = "Show All Tasks";
                ViewData["Head"] = "All Tasks";
                Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));
                List<Models.Task> tasks = context.Tasks.Where(x => x.List.AccountId == acc.Id).ToList();
                List<Models.List> lists = context.Lists.Where(x => x.AccountId == acc.Id).ToList();
                ViewBag.Tags= context.Tags.ToList();
                ViewBag.Tasks = tasks;
                ViewBag.Lists = lists;
            }
            return View();
        }
        [HttpPost]
        public string insert(Models.Task input)
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                context.Tasks.Add(input);
                context.SaveChanges();

            }
            return input.ToString();
        }

        [HttpPost]
        public string GetData(int taskId)
        {
            string jsonString = "";
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Models.Task result = context.Tasks.FirstOrDefault(x => x.Id == taskId);

                if (result != null)
                {
                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    };
                    jsonString = JsonConvert.SerializeObject(result, settings);
                }
            }
            return jsonString;
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Models.Task result = context.Tasks.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    context.Remove(result);
                    context.SaveChanges();
                }

            }
            return Redirect("/task/show");
        }
        [HttpPost]
        public string Update(Models.Task input, List<int> tagIds)
        {
            string jsonString = "";
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Models.Task task = context.Tasks.FirstOrDefault(x => x.Id == input.Id);
                if (task != null)
                {
                    task.Name = input.Name;
                    task.Description = input.Description;
                    task.Status = input.Status;
                    task.ListId = input.ListId;
                    task.DueDate = input.DueDate;

                    // Remove existing tags

                    List<TagsTask> tagsTasksToDelete = context.TagsTasks.Where(tt => tt.TaskId == task.Id).ToList();
                    context.TagsTasks.RemoveRange(tagsTasksToDelete);

                    ICollection<TagsTask> tagsTasks= new List<TagsTask>();
                    for (int i = 0; i < tagIds.Count(); i++)
                    {
                        TagsTask tagTask = new TagsTask();
                        tagTask.TagsId = tagIds[i];
                        tagTask.TaskId = task.Id;
                        tagsTasks.Add(tagTask);
                    }

                    task.TagsTasks = tagsTasks;
                    task.SubTasks = input.SubTasks;
                    context.SaveChanges();
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                jsonString = JsonConvert.SerializeObject(input, settings);
                return jsonString;
            }
        }
        [HttpGet]
        public IActionResult ListShow(int id)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));

                List list1 = context.Lists.FirstOrDefault(x => x.AccountId == acc.Id && x.Id == id);
                if (list1 == null)
                {
                    return NotFound();
                }
                List<Models.Task> tasks = context.Tasks.Where(x => x.List.Id == list1.Id).ToList();
                List<Models.List> lists = context.Lists.Where(x => x.AccountId == acc.Id).ToList();

                ViewData["Title"] = list1.Name;
                ViewData["Head"] = list1.Name;

                ViewBag.Tags= context.Tags.ToList();
                ViewBag.ShowList = list1.Id;
                ViewBag.Tasks = list1.Tasks;
                ViewBag.Lists = lists;
            }
            return View();
        }
        [HttpGet]
        public IActionResult Time(int id, int time)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));

                List<Models.Task> tasks = context.Tasks.ToList();
                if (time == 0) // today
                {
                    DateTime now = DateTime.Now;
                    tasks = context.Tasks.Where(x => x.DueDate == now).ToList();
                    ViewData["Title"] = "Today";
                    ViewData["Head"] = "Today";
                }
                else if (time == 1) // upcomming
                {
                    DateTime now = DateTime.Now;
                    tasks = context.Tasks.Where(x => x.DueDate > now).ToList();
                    ViewData["Title"] = "Up Comming";
                    ViewData["Head"] = "Up Comming";
                }
                else if (time == -1) // in the pass
                {
                    DateTime now = DateTime.Now;
                    tasks = context.Tasks.Where(x => x.DueDate < now).ToList();
                    ViewData["Title"] = "Missed";
                    ViewData["Head"] = "Missed";
                }
                else // not found 
                {
                    return NotFound();
                }
                ViewBag.time = 0;

                ViewBag.Tags= context.Tags.ToList();
                ViewBag.Tasks = tasks;
                List<Models.List> lists = context.Lists.Where(x => x.AccountId == acc.Id).ToList();
                ViewBag.Lists = lists;
                return View("/Views/Task/ListShow.cshtml");
            }
        }


        [HttpGet]
        public string CheckStatus(int id)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {

                Models.Task result = context.Tasks.FirstOrDefault(x => x.Id == id);
                if (result != null)
                {
                    result.Status = !result.Status;
                    context.SaveChanges();
                }
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
                string jsonString = JsonConvert.SerializeObject(result, settings);
                return jsonString;
            }
        }
    }

}
