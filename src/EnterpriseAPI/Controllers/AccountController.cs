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

        public async Task<JsonResult> Login()
        {
            string name = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string lastName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            await user.Create(eventHandler, db, name, lastName, emailAddress);
            return Json(mess);
        }

        public JsonResult Logout()
        {
            return Json("You has been logout successfuly");
        }

        [HttpPut]
        public async Task<JsonResult> Put(string address)
        {
            //HttpContext.Session.SetString("Name", "Mike");
            //var value = HttpContext.Session.GetString("Name");
            //CookieHeaderValue cookie = Request.Headers.GetCookies("person").FirstOrDefault();
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var c = await user.Get(db, emailAddress);
            await user.Update(eventHandler, db, c.userId, address);
            return Json(mess);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name)
        {
            string emailAddress = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var c = await user.Get(db, emailAddress);
            await user.Delete(eventHandler, db, c.userId);
            return Json(mess);
        }

    }
}
