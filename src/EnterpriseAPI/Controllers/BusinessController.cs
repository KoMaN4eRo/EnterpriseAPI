using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models;
using EnterpriseAPI.Models.BusinessModel;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BusinessController : Controller
    {

        private IBusiness business;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, BusinessArgs e)
        {
            mess = e.message;
        }

        public BusinessController(ApplicationContext context, IBusiness business)
        {
            this.business = business;
            db = context;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string countryId)
        {
            await business.Create(eventHandler, db, name, int.Parse(countryId));
            return Json(mess);
        }

        [HttpGet]
        public async Task<JsonResult> ExpandAll(string countryId)
        {
            return Json(await business.ExpandAll(db, int.Parse(countryId)));
        }

        [HttpGet]
        public async Task<JsonResult> Get(string countryId)
        {
            return Json(await business.Get(db, int.Parse(countryId)));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string countryId, string id, string name = null)
        {
            await business.Update(eventHandler, db, int.Parse(countryId), int.Parse(id), name);
            return Json(mess);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string countryId)
        {
            await business.Delete(eventHandler, db, name, int.Parse(countryId));
            return Json(mess);
        }
    }
}