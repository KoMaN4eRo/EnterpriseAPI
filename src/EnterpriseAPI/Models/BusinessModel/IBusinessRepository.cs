using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public interface IBusinessRepository
    {
        Task Create(ApplicationContext db, Business business);
        Task<List<Business>> ExpandAll(ApplicationContext db, int countryId);
        Task<List<Business>> Get(ApplicationContext db, int countryId);
        Task Update(ApplicationContext db, int countryId, int id, string name);
        Task Delete(ApplicationContext db, string name, int countryId);
    }
}
