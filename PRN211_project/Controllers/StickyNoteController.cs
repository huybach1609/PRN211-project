using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN211_project.Fillters;
using PRN211_project.Models;
namespace PRN211_project.Controllers
{
    [TypeFilter(typeof(AuthenticationFillter))]
    public class StickyNoteController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "StickyNote";

            Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));

            List<StickyNote> listS = PRN211_projectContext.Ins.StickyNotes.Where(stick => stick.AccountId == acc.Id).ToList();
            ViewBag.listS = listS;
            ViewBag.accId= acc.Id;
            return View("/Views/StickyNote/Index.cshtml");
        }
 
        public IActionResult Add(StickyNote sticky)
        {
            StickyNote stick= PRN211_projectContext.Ins.StickyNotes.SingleOrDefault(x => x.Id == sticky.Id); //returns a single item.
            if (stick == null)
            {
                PRN211_projectContext.Ins.StickyNotes.Add(sticky);
                PRN211_projectContext.Ins.SaveChanges();
            }
            return Redirect("/stickynote");
        }

        [HttpPost]
        public IActionResult Update(StickyNote sticky)
        {
            ViewBag.anoc = "update";
            StickyNote result = PRN211_projectContext.Ins.StickyNotes.FirstOrDefault(x => sticky.Id == x.Id);


            if (result != null)
            {
                result.Name = sticky.Name;
                result.Details = sticky.Details;
                PRN211_projectContext.Ins.SaveChanges();
            }

            return Redirect("/stickynote");
        }

        public IActionResult Delete(int id)
        {
            StickyNote stick = PRN211_projectContext.Ins.StickyNotes.SingleOrDefault(x => x.Id == id); //returns a single item.

            if (stick != null)
            {
                PRN211_projectContext.Ins.StickyNotes.Remove(stick);
                PRN211_projectContext.Ins.SaveChanges();
            }
            return Redirect("/stickynote");
        }
    }
}
