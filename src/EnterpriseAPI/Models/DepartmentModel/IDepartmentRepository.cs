using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public interface IDepartmentRepository
    {
        Task Create(ApplicationContext db, Department department);
        Task<List<Department>> Get(ApplicationContext db, int offeringId);
        Task Update(ApplicationContext db, int offeringId, int id, string name);
        Task Delete(ApplicationContext db, string name, int offeringId);
    }
}
