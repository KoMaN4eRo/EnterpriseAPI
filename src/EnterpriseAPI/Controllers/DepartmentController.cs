using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.DepartmentModel;
using EnterpriseAPI.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DepartmentController : Controller
    {
        private IDepartment department;
        private ApplicationContext db;
        private string mess;

        private void eventHandler(object sender, DepartmentArgs e)
        {
            mess = e.message;
        }

        public DepartmentController(ApplicationContext context, IDepartment department)
        {
            this.department = department;
            db = context;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string offeringId)
        {
            string controlll = User.Identity.Name;
            if (controlll != null)
            {
                await department.Create(eventHandler, db, name, int.Parse(offeringId));
                return Json(mess);
            }
            else
            {
                return Json("Error. Please Authenticate via social network");
            }
        }

        [HttpGet]
        public async Task<JsonResult> Get(string offeringId)
        {
            return Json(await department.Get(db, int.Parse(offeringId)));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string offeringId, string id, string name = null)
        {
            string controlll = User.Identity.Name;
            if (controlll != null)
            {
                await department.Update(eventHandler, db, int.Parse(offeringId), int.Parse(id), name);
                return Json(mess);
            }
            else
            {
                return Json("Error. Please Authenticate via social network");
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string offeringId)
        {
            string controlll = User.Identity.Name;
            if (controlll != null)
            {
                await department.Delete(eventHandler, db, name, int.Parse(offeringId));
                return Json(mess);
            }
            else
            {
                return Json("Error. Please Authenticate via social network");
            }
        }
    }
}
