using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CountryController : Controller
    {

        private ICountry country;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, CountryArgs e)
        {
            mess = e.message;
        }

        public CountryController(ApplicationContext context, ICountry country)
        {
            this.country = country;
            db = context;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string countryCode, string orgId)
        {
            string controlll = User.Identity.Name;
            if (controlll != null)
            {
                await country.Create(eventHandler, db, name, int.Parse(countryCode), int.Parse(orgId));
                return Json(mess);
            }
            else
            {
                return Json("Error. Please Authenticate via social network");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ExpandAll(string orgId)
        {
            var c = await country.ExpandAll(db, int.Parse(orgId));
            return Json(c);
        }

        [HttpGet]
        public async Task<JsonResult> Get(string orgId)
        {
            var c = await country.Get(db, int.Parse(orgId));
            return Json(c);
        }

        [HttpPut]
        public async Task<JsonResult> Put(string orgId, string id, string name = null, string code = null)
        {
            string controlll = User.Identity.Name;
            if (controlll != null)
            {
                await country.Update(eventHandler, db, int.Parse(orgId), int.Parse(id), name, int.Parse(code));
                return Json(mess);
            }
            else
            {
                return Json("Error. Please Authenticate via social network");
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string orgId)
        {
            string controlll = User.Identity.Name;
            if (controlll != null)
            {
                await country.Delete(eventHandler, db, name, int.Parse(orgId));
                return Json(mess);
            }
            else
            {
                return Json("Error. Please Authenticate via social network");
            }
        }
    }
}
