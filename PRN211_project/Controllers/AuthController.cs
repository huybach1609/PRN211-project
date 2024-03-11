using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN211_project.Fillters;
using PRN211_test.Enums;
using PRN211_project.Models;

namespace PRN211_test.Controllers
{
    public class AuthController : Controller
    {
        [TypeFilter(typeof(AuthFilter))]
        public IActionResult Index()
        {
            ViewData["Title"] = "Begin";
            return View();
        }

        [TypeFilter(typeof(AuthFilter))]
        [HttpGet]
        public IActionResult SignUp()
        {
            ViewData["Title"] = "Sign Up";


            return View();
        }

        [TypeFilter(typeof(AuthFilter))]
        [HttpPost]
        public IActionResult SignUp(Account acc)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {
                acc.Roll = (int)RollType.User;
                context.Accounts.Add(acc);
                context.SaveChanges();
                ViewBag.mess = "Sign up success";
            }
            return View();

        }


        [TypeFilter(typeof(AuthFilter))]
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login page";
            return View();
        }

        [TypeFilter(typeof(AuthFilter))]
        [HttpPost]
        public IActionResult Login(Account accI)
        {
            if (accI != null)
            {
                if (CheckAcc(accI) != null)
                {
                    Account fullacc = CheckAcc(accI);

                    var settings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    };

                    var userJson = JsonConvert.SerializeObject(fullacc, settings);
                    HttpContext.Session.SetString("sesUser", userJson);
                    if (fullacc.Roll == (int)RollType.Admin)
                    {
                        return RedirectToAction("list", "account");
                    }
                    return RedirectToAction("", "home");
                }
            }
            ViewBag.Mess = "login fail";
            return View();
        }


        [TypeFilter(typeof(AuthenticationFillter))]
        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("sesUser");
            return RedirectToAction("");
        }


        public Account CheckAcc(Account acc)
        {
            using (PRN211_projectContext context = new PRN211_projectContext())
            {

                List<Account> Accounts = context.Accounts.ToList();

                foreach (Account a in Accounts)
                {
                    if (a.UserName.Equals(acc.UserName) && a.Password.Equals(acc.Password))
                    {
                        return a;
                    }
                }
            }
            return null;
        }
    }
}
