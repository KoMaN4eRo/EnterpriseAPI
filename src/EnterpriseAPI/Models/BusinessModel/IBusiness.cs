using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public interface IBusiness
    {
        Task Create(BusinessHandler handler, ApplicationContext db, string name, int countryId);
        Task<List<Business>> ExpandAll(ApplicationContext db, int countryId);
        Task<List<Business>> Get(ApplicationContext db, int countryId);
        Task Update(BusinessHandler handler, ApplicationContext db, int countryId, int id, string name);
        Task Delete(BusinessHandler handler, ApplicationContext db, string name, int countryId);
    }
}
