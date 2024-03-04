using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PRN211_test.Enums;
using PRN211_project.Models;
using PRN211_project.Fillters;
using Microsoft.AspNetCore.Http;

namespace PRN211_test.Controllers
{
    public class AccountController : Controller
    {
        // Views/Account/Index.cshtml
        // path : account/index
        public IActionResult Index()
        {
            return View();
        }
        // account/add
        [HttpGet]
        [TypeFilter(typeof(AuthenticationFillter))]
        public IActionResult Update(){
            var sesUserJson = HttpContext.Session.GetString("sesUser");
            Account sesUser = JsonConvert.DeserializeObject<Account>(sesUserJson);
            ViewData["Title"] = sesUser.FullName;
            return View(sesUser);
        }
        [HttpPost]
        [TypeFilter(typeof(AuthenticationFillter))]
        public IActionResult Update(Account acc) {
            // get user ses
            var sesUserJson = HttpContext.Session.GetString("sesUser");
            Account sesUser = JsonConvert.DeserializeObject<Account>(sesUserJson);

            var result = PRN211_projectContext.Ins.Accounts.SingleOrDefault(a => a.Id == sesUser.Id);
            if (result != null)
            {
                result.UserName = acc.UserName;
                result.FullName = acc.FullName;
                result.Email = acc.Email;
                result.Password = acc.Password;
                PRN211_projectContext.Ins.SaveChanges();
            }
            
            // update sesion User
            HttpContext.Session.Remove("sesUser");
            var userJson = JsonConvert.SerializeObject(result);
            HttpContext.Session.SetString("sesUser", userJson);

            return Update();

        }




        [TypeFilter(typeof(AuthenticationFillter))]
        [TypeFilter(typeof(AdminFilter))]
        public IActionResult List()
        {
            //có 3 cách để gửi controller tới view
            // viewbag, viewdata, model
            // gửi bằng view bag
            // gửi bằng viewData
            List<Account> listA = PRN211_projectContext.Ins.Accounts.ToList();
            ViewData["listAccount"] = listA;
            return View(listA);
        }



    }
}
