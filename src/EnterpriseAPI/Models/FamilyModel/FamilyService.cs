using EnterpriseAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.FamilyModel
{
    public class FamilyService: IFamilyService
    {
        private IFamilyRepository familyRepository;
        private ApplicationContext dbContext;
        private IValidation validate;
        public FamilyService(IFamilyRepository _family, ApplicationContext context, IValidation _validate)
        {
            familyRepository = _family;
            dbContext = context;
            validate = _validate;
        }

        public async Task<Dictionary<string, string>> CreateFamily(string name, string businessId)
        {

            var result = await validate.CheckId(businessId, "Business", "Create", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Family", result, "Create", businessId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await familyRepository.Create(dbContext, new Family() { familyName = name, businessId = int.Parse(businessId)});
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> UpdateFamily(string businessId, string id, string name = null)
        {
            var result = await validate.CheckId(businessId, "Business", "Update", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckId(id, "Family", "Update", result);
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Family", result, "Update", businessId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await familyRepository.Update(dbContext, int.Parse(businessId), int.Parse(id), name);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> DeleteFamily(string name, string businessId)
        {
            var result = await validate.CheckId(businessId, "Business", "Delete", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Family", result, "Delete", businessId);

            if (!result.modelValid)
                return result.modelState;
            try
            {
                await familyRepository.Delete(dbContext, name, int.Parse(businessId));
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<object> ExpandAll(string businessId)
        {
            var result = await validate.CheckId(businessId, "Business", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await familyRepository.ExpandAll(dbContext, int.Parse(businessId));
            }

            catch
            {
                return result.modelState;
            }
        }

        public async Task<object> Get(string businessId)
        {
            var result = await validate.CheckId(businessId, "Business", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await familyRepository.Get(dbContext, int.Parse(businessId));
            }

            catch
            {
                return result.modelState;
            }
        }
    }

    public interface IFamilyService
    {
        Task<Dictionary<string, string>> CreateFamily(string name, string businessId);
        Task<Dictionary<string, string>> UpdateFamily(string countryId, string id, string name = null);
        Task<Dictionary<string, string>> DeleteFamily(string name, string countryId);
        Task<object> ExpandAll(string countryId);
        Task<object> Get(string countryId);
    }
}
