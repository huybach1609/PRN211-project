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
            ViewData["Title"] = "List View";
            Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));
            List<Models.List> lists = PRN211_projectContext.Ins.Lists.Where(x => x.AccountId == acc.Id).ToList();
            ViewBag.accId= acc.Id; 
            ViewBag.lists = lists; 
            return View();
        }

        [HttpPost]
        public IActionResult Add(List l) {
            Models.List list = PRN211_projectContext.Ins.Lists.SingleOrDefault(x => x.Id == l.Id);

            if (list == null)
            {
                PRN211_projectContext.Ins.Lists.Add(l);
                PRN211_projectContext.Ins.SaveChanges();
            }
            return Redirect("/list");
        }

        [HttpPost]
        public IActionResult Update(List l) {
            Models.List list = PRN211_projectContext.Ins.Lists.SingleOrDefault(x => x.Id == l.Id);
            if (list != null)
            {
                list.Name =l.Name;
                PRN211_projectContext.Ins.SaveChanges();
            }
            return Redirect("/list");
        }

        public IActionResult Delete(int id)
        {

            Models.List list = PRN211_projectContext.Ins.Lists.SingleOrDefault(x => x.Id == id);
            if (list != null)
            {
                PRN211_projectContext.Ins.Lists.Remove(list);
                PRN211_projectContext.Ins.SaveChanges();
            }
            return Redirect("/list");
        }

    }
}
