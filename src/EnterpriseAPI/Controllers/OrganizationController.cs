using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EnterpriseAPI.Models.OrganizationModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using System.Net;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Security.Claims;
using EnterpriseAPI.Validation.ValidateOrganization.Code;

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class OrganizationController : Controller
    {
        private IOrganizationService organizationService;
        private IValidateCode validateCode;
        public OrganizationController(IOrganizationService _orgService, IValidateCode _validate)
        {
            organizationService = _orgService;
            validateCode = _validate;
        }

        //[HttpPost]
        //public async Task<JsonResult> Create(string name, string code, string type)
        //{
        //    string userName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
        //    string userLastName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
        //    if (code == null) code = "0";
        //    var t = await organizationService.CreateOrganization(name, code, type, $"{userName} {userLastName}");
        //    return Json(t);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Organization org)
        {
            bool valid = true;
            if (!(await validateCode.IsValidCode(org.organizationCode)))
            {
                ModelState.AddModelError("organizationCode", $"OrganizationCode {org.organizationCode} is already exist");
                valid = false;
            }

            if (org == null || !ModelState.IsValid)
            {
                valid = false;
            }

            if (!valid) return BadRequest(ModelState);

            string userName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string userLastName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            var t = await organizationService.CreateOrganization(org.organizationName, org.organizationCode.ToString("D"), org.organizationType, $"{userName} {userLastName}");
            
            return Ok(t);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> ExpandAll(string id)
        {
            return Json(await organizationService.ExpandAll(id));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return Json(await organizationService.Get());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetCurrentOwnerOrganization()
        {
            string userName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string userLastName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            string owner = $"{userName} {userLastName}";
            return Json(await organizationService.GetCurrentOwnerOrganization(owner));
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetByType(string type)
        {
            return Json(await organizationService.GetByType(type));
        }

        [HttpPut]
        public async Task<JsonResult> Put(string id, string name = null, string code = null, string type = null)
        {
            return Json(await organizationService.UpdateOrganization(id, name, code, type)); 
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(string name)
        {
            return Json(await organizationService.DeleteOrganizaiotn(name));
        }
    }
}
