using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.OfferingModel;
using EnterpriseAPI.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OfferingController : Controller
    {

        private IOffering offering;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, OfferingArgs e)
        {
            mess = e.message;
        }

        public OfferingController(ApplicationContext context, IOffering offering)
        {
            this.offering = offering;
            db = context;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string familyId)
        {
            await offering.Create(eventHandler, db, name, int.Parse(familyId));
            return Json(mess);
        }

        [HttpGet]
        public async Task<JsonResult> ExpandAll(string familyId)
        {
            return Json(await offering.ExpandAll(db, int.Parse(familyId)));
        }

        [HttpGet]
        public async Task<JsonResult> Get(string familyId)
        {
            return Json(await offering.Get(db, int.Parse(familyId)));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string familyId, string id, string name = null)
        {
            await offering.Update(eventHandler, db, int.Parse(familyId), int.Parse(id), name);
            return Json(mess);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string familyId)
        {
            await offering.Delete(eventHandler, db, name, int.Parse(familyId));
            return Json(mess);
        }
    }
}
