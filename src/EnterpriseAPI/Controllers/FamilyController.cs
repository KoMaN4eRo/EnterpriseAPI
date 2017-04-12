using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.FamilyModel;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class FamilyController : Controller
    {
        private IFamilyService familyService;
        public FamilyController(IFamilyService _familyService)
        {
            familyService = _familyService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string businessId)
        {
            return Json(await familyService.CreateFamily(name, businessId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> ExpandAll(string businessId)
        {
            return Json(await familyService.ExpandAll(businessId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get(string businessId)
        {
            return Json(await familyService.Get(businessId));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string businessId, string id, string name = null)
        {
              return Json(await familyService.UpdateFamily(businessId, id, name));
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string businessId)
        {
               return Json(await familyService.DeleteFamily(name, businessId));
        }
    }
}
