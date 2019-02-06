using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Identity_MVC.Models;
using System.Threading.Tasks;

namespace Identity_MVC.Controllers
{
    public class AccountController : Controller
    {

        public UserManager<IdentityUser> UserManager => HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();
        public SignInManager<IdentityUser, string> signInManager 
                => HttpContext.GetOwinContext().Get<SignInManager<IdentityUser, string>>();

        // GET: Account
        [HttpGet]
        public  ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            var signInStatus = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
            switch (signInStatus)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    ModelState.AddModelError("", "Invalid credentials....");
                    return View(model);
                default:
                    ModelState.AddModelError("", "Invalid credentials");
                    return View(model);
                        
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            //var identityUser = await UserManager.FindByNameAsync(model.Username);

            //if(identityUser != null)
            //{
            //    return RedirectToAction("Index","Home");
            //}

            var result = await UserManager.CreateAsync(new IdentityUser(model.Username), model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", result.Errors.FirstOrDefault());

            return View(model);


        }
    }
}