using EnterpriseAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public class BusinessService : IBusinessService
    {
        private IBusinessRepository businessRepository;
        private ApplicationContext dbContext;
        private IValidation validate;
        public BusinessService(IBusinessRepository _business, ApplicationContext context, IValidation _validate)
        {
            businessRepository = _business;
            dbContext = context;
            validate = _validate;
        }

        public async Task<Dictionary<string, string>> CreateBusiness(string name, string countryId)
        {
            var result = await validate.CheckId(countryId, "Country", "Create", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Business", result, "Create", countryId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await businessRepository.Create(dbContext, new Business() { businessName = name, countryId = int.Parse(countryId)});
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> UpdateBusiness(string countryId, string id, string name = null)
        {
            var result = await validate.CheckId(countryId, "Country", "Update", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckId(id, "Business", "Update", result);
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Business", result, "Update", countryId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await businessRepository.Update(dbContext, int.Parse(countryId), int.Parse(id), name);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> DeleteBusiness(string name, string countryId)
        {
            var result = await validate.CheckId(countryId, "Country", "Delete", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Business", result, "Delete", countryId);

            if (!result.modelValid)
                return result.modelState;
            try
            {
                await businessRepository.Delete(dbContext, name, int.Parse(countryId));
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<object> ExpandAll(string countryId)
        {
            var result = await validate.CheckId(countryId, "Country", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await businessRepository.ExpandAll(dbContext, int.Parse(countryId));
            }

            catch
            {
                return result.modelState;
            }
        }

        public async Task<object> Get(string countryId)
        {
            var result = await validate.CheckId(countryId, "Country", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await businessRepository.Get(dbContext, int.Parse(countryId));
            }

            catch
            {
                return result.modelState;
            }
        }
    }

    public interface IBusinessService
    {
        Task<Dictionary<string, string>> CreateBusiness(string name, string countryId);
        Task<Dictionary<string, string>> UpdateBusiness(string countryId, string id, string name = null);
        Task<Dictionary<string, string>> DeleteBusiness(string name, string countryId);
        Task<object> ExpandAll(string countryId);
        Task<object> Get(string countryId);
    }
}
