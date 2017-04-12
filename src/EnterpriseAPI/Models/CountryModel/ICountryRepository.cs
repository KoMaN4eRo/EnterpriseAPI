using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.CountryModel
{
    public interface ICountryRepository
    {
        Task Create(ApplicationContext db, Country country);
        Task<List<Country>> ExpandAll(ApplicationContext db, int organizationId);
        Task<List<Country>> Get(ApplicationContext db, int organizationId);
        Task Update(ApplicationContext db, int organizationId, int id, string name = null, int code = 0);
        Task Delete(ApplicationContext db, string name, int organizationId);
    }
}
