using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.FamilyModel
{
    public interface IFamilyRepository
    {
        Task Create(ApplicationContext db, Family family);
        Task<List<Family>> ExpandAll(ApplicationContext db, int businessId);
        Task<List<Family>> Get(ApplicationContext db, int businessId);
        Task Update(ApplicationContext db, int businessId, int id, string name);
        Task Delete(ApplicationContext db, string name, int businessId);
    }
}
