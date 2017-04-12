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

namespace EnterpriseAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class OrganizationController :Controller
    {
        private IOrganizationService organizationService; 
        public OrganizationController(IOrganizationService _orgService )
        {
            organizationService = _orgService;
        }

        [HttpPost]
        public async Task<JsonResult> Create(string name, string code, string type)
        {
            string userName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            string userLastName = User.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
            if (code == null) code = "0";
            var t = await organizationService.CreateOrganization(name, code, type, $"{userName} {userLastName}");
            return Json(t);
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
