using EnterpriseAPI.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public class DepartmentService: IDepartmentService
    {
        private IDepartmentRepository departmentRepository;
        private ApplicationContext dbContext;
        private IValidation validate;
        public DepartmentService(IDepartmentRepository _departmentRepository, ApplicationContext context, IValidation _validate)
        {
            departmentRepository = _departmentRepository;
            dbContext = context;
            validate = _validate;
        }
        
        public async Task<Dictionary<string, string>> CreateDepartment(string name, string offeringId)
        {
            var result = await validate.CheckId(offeringId, "Offering", "Create", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Department", result, "Create", offeringId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await departmentRepository.Create(dbContext, new Department() { departmentName = name, offeringId = int.Parse(offeringId)});
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> UpdateDepartment(string offeringId, string id, string name = null)
        {
            var result = await validate.CheckId(offeringId, "Offering", "Update", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckId(id, "Department", "Update", result);
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Department", result, "Update", offeringId);
            if (!result.modelValid)
                return result.modelState;
            try
            {
                await departmentRepository.Update(dbContext, int.Parse(offeringId), int.Parse(id), name);
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<Dictionary<string, string>> DeleteDepartment(string name, string offeringId)
        {
            var result = await validate.CheckId(offeringId, "Offering", "Delete", new ModelStateHandler());
            if (!result.modelValid)
            {
                return result.modelState;
            }
            result = await validate.CheckName(name, "Department", result, "Delete", offeringId);

            if (!result.modelValid)
                return result.modelState;
            try
            {
                await departmentRepository.Delete(dbContext, name, int.Parse(offeringId));
            }

            catch
            {
                return result.modelState;
            }

            return result.modelState;
        }

        public async Task<object> Get(string offeringId)
        {
            var result = await validate.CheckId(offeringId, "Offering", "Get", new ModelStateHandler());

            if (!result.modelValid)
                return result.modelState;
            try
            {
                return await departmentRepository.Get(dbContext, int.Parse(offeringId));
            }

            catch
            {
                return result.modelState;
            }
        }
    }

    public interface IDepartmentService
    {
        Task<Dictionary<string, string>> CreateDepartment(string name, string offeringId);
        Task<Dictionary<string, string>> UpdateDepartment(string familyId, string id, string name = null);
        Task<Dictionary<string, string>> DeleteDepartment(string name, string familyId);
        Task<object> Get(string familyId);
    }
}
