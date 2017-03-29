using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public interface IDepartment
    {
        Task Create(DepartmentHandler handler, ApplicationContext db, string name, int offeringId);
        Task<List<Department>> Get(ApplicationContext db, int offeringId);
        Task Update(DepartmentHandler handler, ApplicationContext db, int offeringId, int id, string name);
        Task Delete(DepartmentHandler handler, ApplicationContext db, string name, int offeringId);
    }
}
