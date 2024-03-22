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
            using (PRN211_projectContext context = new PRN211_projectContext())
            {

                ViewData["Title"] = "StickyNote";

                Account acc = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("sesUser"));

                List<StickyNote> listS = context.StickyNotes.Where(stick => stick.AccountId == acc.Id).ToList();
                ViewBag.listS = listS;
                ViewBag.accId = acc.Id;
            }
            return View("/Views/StickyNote/Index.cshtml");
        }

        public IActionResult Add(StickyNote sticky)
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                StickyNote stick = context.StickyNotes.SingleOrDefault(x => x.Id == sticky.Id); //returns a single item.
                if (stick == null)
                {
                    context.StickyNotes.Add(sticky);
                    context.SaveChanges();
                }
            }
            return Redirect("/stickynote");
        }

        [HttpPost]
        public IActionResult Update(StickyNote sticky)
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                ViewBag.anoc = "update";
                StickyNote result = context.StickyNotes.FirstOrDefault(x => sticky.Id == x.Id);


                if (result != null)
                {
                    result.Name = sticky.Name;
                    result.Details = sticky.Details;
                    context.SaveChanges();
                }
            }

            return Redirect("/stickynote");
        }

        public IActionResult Delete(int id)
        {

            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                StickyNote stick = context.StickyNotes.SingleOrDefault(x => x.Id == id); //returns a single item.

                if (stick != null)
                {
                    context.StickyNotes.Remove(stick);
                    context.SaveChanges();
                }
            }
            return Redirect("/stickynote");
        }
    }
}
