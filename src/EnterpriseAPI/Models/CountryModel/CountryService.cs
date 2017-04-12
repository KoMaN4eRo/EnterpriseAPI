using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using EnterpriseAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.CountryModel
{
    public class CountryService : ICountryService
    {
        private ICountryRepository countryRepository;
        private ApplicationContext dbContext;
        private IValidation validate;
        public CountryService(ICountryRepository _country, ApplicationContext context, IValidation _validate)
        {
            countryRepository = _country;
            dbContext = context;
            validate = _validate;
        }

        public async Task<Dictionary<string, string>> CreateCountry(string name, string code, string orgId)
        {
            var result = await validate.CheckId(orgId, "Organization", "Create", new ModelStateHandler());
            if (result.modelValid == false)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Country", result, "Create", orgId);
            result = await validate.CheckCode(code, "Create", "Country", result, orgId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                Country country = new Country() { countryName = name, countryCode = int.Parse(code), organizationId = int.Parse(orgId) };
                await countryRepository.Create(dbContext, country);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> UpdateCountry(string orgId, string id, string name = null, string code = null)
        {
            var result = await validate.CheckId(orgId, "Organization", "Update", new ModelStateHandler());
            if (result.modelValid == false)
            {
                return result.modelState;
            }
            result = await validate.CheckId(id, "Country", "Update", result);
            if (result.modelValid == false)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Country", result, "Update", orgId);
            result = await validate.CheckCode(code, "Update", "Country", result, orgId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                if (validate.IsNull(code)) code = "0";
                await countryRepository.Update(dbContext, int.Parse(orgId), int.Parse(id), name, int.Parse(code));
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> DeleteCountry(string name, string orgId)
        {
            var result = await validate.CheckId(orgId, "Organization", "Delete", new ModelStateHandler());
            if (result.modelValid == false)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Country", result, "Delete", orgId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await countryRepository.Delete(dbContext, name, int.Parse(orgId));
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<object> ExpandAll(string orgId)
        {
            var result = await validate.CheckId(orgId, "Organization", "Get", new ModelStateHandler());
            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await countryRepository.ExpandAll(dbContext, int.Parse(orgId));
            }

            catch
            {
                return result.modelState;
            }
        }

        public async Task<object> Get(string orgId)
        {
            var result = await validate.CheckId(orgId, "Organization", "Get", new ModelStateHandler());
            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await countryRepository.Get(dbContext, int.Parse(orgId));
            }

            catch
            {
                return result.modelState;
            }
        }
    }

    public interface ICountryService
    {
        Task<Dictionary<string, string>> CreateCountry(string name, string code, string orgId);
        Task<Dictionary<string, string>> UpdateCountry(string orgId, string id, string name = null, string code = null);
        Task<Dictionary<string, string>> DeleteCountry(string name, string orgId);
        Task<object> ExpandAll(string orgId); 
        Task<object> Get(string organizationId); 
    }
}
