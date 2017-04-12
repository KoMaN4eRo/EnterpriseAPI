using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public interface IOrganizationRepository
    {
        Task Create(ApplicationContext db, Organization org);
        Task<Organization> ExpandAll(ApplicationContext db, int id);
        Task<List<Organization>> Get(ApplicationContext db);
        Task<List<Organization>> GetCurrentOwnerOrganization(ApplicationContext db, string owner);
        Task<List<Organization>> GetByType(ApplicationContext db, string organizationType);
        Task Update(ApplicationContext db, int id, string name = null, int code = 0, string type = null);
        Task Delete(ApplicationContext db, string name);
    }
}
