using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PRN211_project.Fillters;
using PRN211_test.Enums;
using PRN211_project.Models;

namespace PRN211_test.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Begin";
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewData["Title"] = "Sign Up";


            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Account acc)
        {
            acc.Roll = (int)RollType.User;
            PRN211_projectContext.Ins.Accounts.Add(acc);
            PRN211_projectContext.Ins.SaveChanges();
            ViewBag.mess = "Sign up success";
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["Title"] = "Login page";
            return View();
        }
        [HttpPost]
        public IActionResult Login(Account accI)
        {
            if (accI != null)
            {
                if (CheckAcc(accI) != null)
                {
                    Account fullacc = CheckAcc(accI);

                    var userJson = JsonConvert.SerializeObject(fullacc);
                    HttpContext.Session.SetString("sesUser", userJson);
                    if (fullacc.Roll == (int) RollType.Admin)
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
            List<Account> Accounts = PRN211_projectContext.Ins.Accounts.ToList();

            foreach (Account a in Accounts)
            {
                if (a.UserName.Equals(acc.UserName) && a.Password.Equals(acc.Password))
                {
                    return a;
                }
            }
            return null;
        }
    }
}
