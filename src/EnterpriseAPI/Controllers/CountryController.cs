using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.CountryModel;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class CountryController : Controller
    {
        private ICountryService countryService;
        public CountryController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string code, string orgId)
        {
            return Json(await countryService.CreateCountry(name, code, orgId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> ExpandAll(string orgId)
        {
            return Json(await countryService.ExpandAll(orgId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get(string orgId)
        {
            return Json(await countryService.Get(orgId));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string orgId, string id, string name = null, string code = null)
        {
            return Json(await countryService.UpdateCountry(orgId, id, name, code));
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string orgId)
        {
            return Json(await countryService.DeleteCountry(name, orgId));
        }
    }
}
