using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN211_project.Fillters;
using PRN211_project.Models;

namespace PRN211_project.Controllers
{

    [TypeFilter(typeof(AuthenticationFillter))]
    public class ListController : Controller
    {
        public IActionResult Index()
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                ViewData["Title"] = "List View";
                Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));
                List<Models.List> lists = context.Lists.Where(x => x.AccountId == acc.Id).ToList();
                ViewBag.accId = acc.Id;
                ViewBag.lists = lists;
            }
            return View();
        }

        [HttpPost]
        public IActionResult Add(List l)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {

                Models.List list = context.Lists.SingleOrDefault(x => x.Id == l.Id);

                if (list == null)
                {
                    context.Lists.Add(l);
                    context.SaveChanges();
                }

            }
            return Redirect("/list");
        }

        [HttpPost]
        public IActionResult Update(List l)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Models.List list = context.Lists.SingleOrDefault(x => x.Id == l.Id);
                if (list != null)
                {
                    list.Name = l.Name;
                    context.SaveChanges();
                }
            }
            return Redirect("/list");
        }

        public IActionResult Delete(int id)
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                Models.List list = context.Lists.SingleOrDefault(x => x.Id == id);
                if (list != null)
                {
                    context.Lists.Remove(list);
                    context.SaveChanges();
                }
            }
            return Redirect("/list");
        }

    }
}
