using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public class DepartmentRepository : IDepartmentRepository
    {
        public async Task Create(ApplicationContext db, Department department)
        {
            db.department.Add(department);
            await db.SaveChangesAsync();
        }

        public async Task Update(ApplicationContext db, int offeringId, int id, string name)
        {
            Department department = await db.department.Where(dep => dep.departmentId == id).FirstOrDefaultAsync();
            if (name != null) department.departmentName = name;
            db.department.Update(department);
            await db.SaveChangesAsync();
        }

        public async Task Delete(ApplicationContext db, string name, int offeringId)
        {
            Department department = await db.department.Where(dep => dep.departmentName == name && dep.offeringId == offeringId).FirstOrDefaultAsync();
            db.department.Remove(department);
            await db.SaveChangesAsync();
        }

        public async Task<List<Department>> Get(ApplicationContext db, int offeringId)
        {
            List<Department> department = await db.department.Where(dep => dep.offeringId == offeringId)
                                                             .ToListAsync();
            return department;
        }
    }
}
