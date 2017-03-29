using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public interface IOrganization
    {
        Task Create(OrganizationHandler create, ApplicationContext db, string name, int code, string type, string owner);
        Task<List<Organization>> ExpandAll(ApplicationContext db);//Get all hierarchy
        Task<List<Organization>> Get(ApplicationContext db);//get only Organizations
        Task<List<Organization>> GetByType(ApplicationContext db, string organizationType);
        Task Update(OrganizationHandler update, ApplicationContext db, int id, string name = null, int code = 0, string type = null);
        Task Delete(OrganizationHandler delete, ApplicationContext db, string name);
    }
}
