using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OfferingModel
{
    public interface IOfferingRepository
    {
        Task Create(ApplicationContext db, Offering offering);
        Task<List<Offering>> ExpandAll(ApplicationContext db, int familyId);
        Task<List<Offering>> Get(ApplicationContext db, int familyId);
        Task Update(ApplicationContext db, int familyId, int id, string name);
        Task Delete(ApplicationContext db, string name, int familyId);
    }
}
