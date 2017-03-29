using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.FamilyModel
{
    public interface IFamily
    {
        Task Create(FamilyHandler handler, ApplicationContext db, string name, int businessId);
        Task<List<Family>> ExpandAll(ApplicationContext db, int businessId);
        Task<List<Family>> Get(ApplicationContext db, int businessId);
        Task Update(FamilyHandler handler, ApplicationContext db, int businessId, int id, string name);
        Task Delete(FamilyHandler handler, ApplicationContext db, string name, int businessId);
    }
}
