using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.BusinessModel;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class BusinessController : Controller
    {

        private IBusinessService businessService;
        public BusinessController(IBusinessService businessService)
        {
            this.businessService = businessService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string countryId)
        {
            return Json(await businessService.CreateBusiness(name, countryId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> ExpandAll(string countryId)
        {
            return Json(await businessService.ExpandAll(countryId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get(string countryId)
        {
            return Json(await businessService.Get(countryId));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string countryId, string id, string name = null)
        {
            return Json(await businessService.UpdateBusiness(countryId, id, name));
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string countryId)
        {
            return Json(await businessService.DeleteBusiness(name, countryId));
        }
    }
} 