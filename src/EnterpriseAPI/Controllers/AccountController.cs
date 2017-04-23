using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using EnterpriseAPI.Models;
using Newtonsoft.Json.Linq;
using EnterpriseAPI.Models.UserModel;
using Microsoft.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private IUser user;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, UserArgs e)
        {
            mess = e.message;
        }

        public AccountController(ApplicationContext context, IUser user)
        {
            this.user = user;
            db = context;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Hello!";
            return View("Index");
        }

        public IActionResult Login()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/api/Account/CompleteLogin" }, "LinkedIn");
        }

        public async Task<JsonResult> CompleteLogin()
        {
            string name = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string lastName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            await user.Create(eventHandler, db, name, lastName, emailAddress);
            return Json("Login complete");
        }
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/api/Account/Result" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public JsonResult Info()
        {
            return Json("Please authenticate via LinkedIn");
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult Result()
        {
            return Json("Logout complete");
        }

        [Authorize]
        [HttpPut]
        public async Task<JsonResult> Put(string address)
        {
            if (address == null)
            {
                return Json("There is no value");
            }
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var c = await user.Get(db, emailAddress);
            await user.Update(eventHandler, db, c.userId, address);
            return Json(mess);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var c = await user.Get(db, emailAddress);
            return Json(c);
        }

        [Authorize]
        [HttpDelete]
        public async Task<JsonResult> Delete()
        {
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var c = await user.Get(db, emailAddress);
            await user.Delete(eventHandler, db, c.userId);
            return Json(mess);
        }

    }
}
