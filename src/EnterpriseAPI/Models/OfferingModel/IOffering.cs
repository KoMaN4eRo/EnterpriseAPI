using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OfferingModel
{
    public interface IOffering
    {
        Task Create(OfferingHandler handler, ApplicationContext db, string name, int familyId);
        Task<List<Offering>> ExpandAll(ApplicationContext db, int familyId);
        Task<List<Offering>> Get(ApplicationContext db, int familyId);
        Task Update(OfferingHandler handler, ApplicationContext db, int familyId, int id, string name);
        Task Delete(OfferingHandler handler, ApplicationContext db, string name, int familyId);
    }
}
