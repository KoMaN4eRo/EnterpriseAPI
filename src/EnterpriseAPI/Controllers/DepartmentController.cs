using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.DepartmentModel;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class DepartmentController : Controller
    {
        private IDepartmentService departmentService;
        public DepartmentController(IDepartmentService _departmentService)
        {
            departmentService = _departmentService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string offeringId)
        { 
            return Json(await departmentService.CreateDepartment(name, offeringId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get(string offeringId)
        {
            return Json(await departmentService.Get(offeringId));
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<JsonResult> Put(string offeringId, string id, string name = null)
        { 
             return Json(await departmentService.UpdateDepartment(offeringId, id, name));
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string offeringId)
        {
                return Json(await departmentService.DeleteDepartment(name, offeringId));
        }
    }
}
