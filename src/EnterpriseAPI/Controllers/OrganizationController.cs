using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models;
using EnterpriseAPI.Models.OrganizationModel;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models.BusinessModel;

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OrganizationController : Controller
    {

        private IOrganization organization;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, OrganizationArgs e)
        {
            mess = e.message;
        }

        public OrganizationController(ApplicationContext context, IOrganization organization)
        {
            this.organization = organization;
            db = context;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string orgCode, string type, string owner)
        {
            await organization.Create(eventHandler, db, name, int.Parse(orgCode), type, owner);
            return Json(mess);
        }

        [HttpGet]
        public async Task<JsonResult> ExpandAll()
        {
            var c = await organization.ExpandAll(db);
            return Json(c);
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var c = await organization.Get(db);
            return Json(c);
        }

        [HttpGet]
        public async Task<JsonResult> GetByType(string organizationType)
        {
            var c = await organization.GetByType(db, organizationType);
            return Json(c);
        }

        [HttpPut]
        public async Task<JsonResult> Put(string id, string name = null, string orgCode = null, string type = null)
        {
            await organization.Update(eventHandler, db, int.Parse(id), name, int.Parse(orgCode), type);
            return Json(mess);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name)
        {
            await organization.Delete(eventHandler, db, name);
            return Json(mess);
        }
    }
}
