using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.CountryModel
{
    public interface ICountry
    {
        Task Create(CountryHandler create, ApplicationContext db, string name, int code, int organizationId);
        Task<List<Country>> ExpandAll(ApplicationContext db, int organizationId);
        Task<List<Country>> Get(ApplicationContext db, int organizationId);
        Task Update(CountryHandler update, ApplicationContext db, int organizationId, int id, string name = null, int code = 0);
        Task Delete(CountryHandler delete, ApplicationContext db, string name, int organizationId);
    }
}
