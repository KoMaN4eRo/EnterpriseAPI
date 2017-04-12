using EnterpriseAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OfferingModel
{
    public class OfferingService : IOfferingService
    {
        private IOfferingRepository offeringRepository;
        private ApplicationContext dbContext;
        private IValidation validate;

        public OfferingService(IOfferingRepository _offeringRepository, ApplicationContext context, IValidation _validate)
        {
            offeringRepository = _offeringRepository;
            dbContext = context;
            validate = _validate;
        }

        public async Task<Dictionary<string, string>> CreateOffering(string name, string familyId)
        {
            var result = await validate.CheckId(familyId, "Family", "Create", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Offering", result, "Create", familyId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await offeringRepository.Create(dbContext, new Offering() { offeringName = name, familyId = int.Parse(familyId)});
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> UpdateOffering(string familyId, string id, string name = null)
        {
            var result = await validate.CheckId(familyId, "Family", "Update", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckId(id, "Offering", "Update", result);
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Offering", result, "Update", familyId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await offeringRepository.Update(dbContext, int.Parse(familyId), int.Parse(id), name);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> DeleteOffering(string name, string familyId)
        {
            var result = await validate.CheckId(familyId, "Family", "Delete", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Offering", result, "Delete", familyId);

            if (!result.modelValid)
                return result.modelState;
            try
            {
                await offeringRepository.Delete(dbContext, name, int.Parse(familyId));
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<object> ExpandAll(string familyId)
        {
            var result = await validate.CheckId(familyId, "Family", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await offeringRepository.ExpandAll(dbContext, int.Parse(familyId));
            }

            catch
            {
                return result.modelState;
            }
        }

        public async Task<object> Get(string familyId)
        {
            var result = await validate.CheckId(familyId, "Family", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await offeringRepository.Get(dbContext, int.Parse(familyId));
            }

            catch
            {
                return result.modelState;
            }
        }
    }

    public interface IOfferingService
    {
        Task<Dictionary<string, string>> CreateOffering(string name, string familyId);
        Task<Dictionary<string, string>> UpdateOffering(string familyId, string id, string name = null);
        Task<Dictionary<string, string>> DeleteOffering(string name, string familyId);
        Task<object> ExpandAll(string familyId);
        Task<object> Get(string familyId);
    }
}
