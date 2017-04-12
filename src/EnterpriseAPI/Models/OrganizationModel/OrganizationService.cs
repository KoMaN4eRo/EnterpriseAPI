using EnterpriseAPI.Validation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public class OrganizationService: IOrganizationService
    {
        private IOrganizationRepository organizationRepository;
        private ApplicationContext dbContext;
        private IValidation validate;

        public OrganizationService(IOrganizationRepository _organization,
                                   ApplicationContext context, 
                                   IValidation _validate)
        {
            organizationRepository = _organization;
            dbContext = context;
            validate = _validate;
        }

        public async Task<Dictionary<string,string>> CreateOrganization(string name, string orgCode, string type, string owner)
        {
            var result = await validate.CheckName(name, "Organization", new ModelStateHandler());
            result = await validate.CheckCode(orgCode, "Create", "Organization", result);
            result = await validate.CheckType(type, result);

            if (!result.modelValid)
                return result.modelState;
            try
            {
                await organizationRepository.Create(dbContext,
                    new Organization()
                    {
                        organizationName = name,
                        organizationCode = int.Parse(orgCode),
                        organizationType = type,
                        Owner = owner
                    });
            }
            
            catch 
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> UpdateOrganization
            (string id, string name = null, string code = null, string type = null)
        {
            var result = await validate.CheckId(id, "Organization", "Update", new ModelStateHandler());
            result = await validate.CheckCode(code, "Update", "Organization", result, id);
            result = await validate.CheckType(type, result, "Update");
            result = await validate.CheckName(name, "Organization", result, "Update", id);
            result = validate.IsNullAll(result, name, code, type);

            if (!result.modelValid)
                return result.modelState;
            try
            {
                await organizationRepository.Update(dbContext, int.Parse(id), name, int.Parse(code), type);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> DeleteOrganizaiotn(string name)
        {
            var result = await validate.CheckName(name, "Organization", new ModelStateHandler(), "Delete");
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await organizationRepository.Delete(dbContext, name);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<object> ExpandAll(string id)
        {
            var result = await validate.CheckId(id, "Organization", "Get", new ModelStateHandler());
            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await organizationRepository.ExpandAll(dbContext, int.Parse(id));
            }

            catch
            {
                return result.modelState;
            }
        }

        public async Task<List<Organization>> GetCurrentOwnerOrganization(string owner)
        {
            return await organizationRepository.GetCurrentOwnerOrganization(dbContext, owner);
        }

        public async Task<List<Organization>> Get()
        {
            return await organizationRepository.Get(dbContext);
        }

        public async Task<object> GetByType(string organizationType)
        {

            var result = await validate.CheckType(organizationType, new ModelStateHandler(), "Get");
            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await organizationRepository.GetByType(dbContext, organizationType);
            }

            catch
            {
                return result.modelState;
            }
        }

    }

    public interface IOrganizationService
    {
        Task<Dictionary<string,string>> CreateOrganization(string name, string orgCode, string type, string owner);
        Task<Dictionary<string, string>> UpdateOrganization(string id, string name = null, string code = null, string type = null);
        Task<Dictionary<string, string>> DeleteOrganizaiotn(string name);
        Task<object> ExpandAll(string id);
        Task<List<Organization>> Get();
        Task<List<Organization>> GetCurrentOwnerOrganization(string owner);
        Task<object> GetByType(string organizationType);
    }
}
