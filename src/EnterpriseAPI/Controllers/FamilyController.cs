using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FamilyController : Controller
    {

        private IFamily family;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, FamilyArgs e)
        {
            mess = e.message;
        }

        public FamilyController(ApplicationContext context, IFamily family)
        {
            this.family = family;
            db = context;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string businessId)
        {
            await family.Create(eventHandler, db, name, int.Parse(businessId));
            return Json(mess);
        }

        [HttpGet]
        public async Task<JsonResult> ExpandAll(string businessId)
        {
            return Json(await family.ExpandAll(db,int.Parse(businessId)));
        }

        [HttpGet]
        public async Task<JsonResult> Get(string businessId)
        {
            return Json(await family.Get(db, int.Parse(businessId)));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string businessId, string id, string name = null)
        {
            await family.Update(eventHandler, db, int.Parse(businessId), int.Parse(id), name);
            return Json(mess);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string businessId)
        {
            await family.Delete(eventHandler, db, name, int.Parse(businessId));
            return Json(mess);
        }
    }
}
