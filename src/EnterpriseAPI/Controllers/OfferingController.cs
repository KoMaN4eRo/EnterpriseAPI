using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.AspNetCore.Authorization;

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class OfferingController : Controller
    {
        private IOfferingService offeringService;
        public OfferingController(IOfferingService _offeringService)
        {
            offeringService = _offeringService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name = null, string familyId = null)
        {
            return Json(await offeringService.CreateOffering(name, familyId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> ExpandAll(string familyId)
        {
            return Json(await offeringService.ExpandAll(familyId));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get(string familyId)
        {
            return Json(await offeringService.Get(familyId));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string familyId, string id, string name = null)
        {
            return Json(await offeringService.UpdateOffering(familyId, id, name));
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name, string familyId)
        {
            return Json(await offeringService.DeleteOffering(name, familyId));
        }
    }
}
