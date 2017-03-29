using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public class Department: IDepartment
    {
        [Key]
        public int departmentId { get; set; }
        [Required]
        public string departmentName { get; set; }
        [Required]
        public int offeringId { get; set; }

        protected internal event DepartmentHandler createDepartment;
        protected internal event DepartmentHandler updateDepartment;
        protected internal event DepartmentHandler deleteDepartment;

        public Department() { }
        
        private void CallEvent(DepartmentArgs e, DepartmentHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(DepartmentArgs e)
        {
            CallEvent(e, createDepartment);
        }

        protected virtual void OnUpdated(DepartmentArgs e)
        {
            CallEvent(e, updateDepartment);
        }

        protected virtual void OnDeleted(DepartmentArgs e)
        {
            CallEvent(e, deleteDepartment);
        }

        public async Task Create(DepartmentHandler handler, ApplicationContext db, string name, int offeringId)
        {
            createDepartment = handler;
            if (await db.department.AnyAsync(dep => dep.departmentName == name && dep.offeringId == offeringId))
            {
                OnCreated(new DepartmentArgs($"Departmetn with name:{name} already exist inside offering with id:{offeringId}"));
            }

            else
            {
                db.department.Add(new Department() { departmentName = name, offeringId = offeringId});
                await db.SaveChangesAsync();
                OnCreated(new DepartmentArgs("200"));
            }
        }

        public async Task Update(DepartmentHandler handler, ApplicationContext db, int offeringId, int id, string name)
        {
            updateDepartment = handler;
            if (await db.department.AnyAsync(dep => dep.departmentName == name && dep.offeringId == offeringId))
            {
                OnUpdated(new DepartmentArgs($"Department with name:{name} already exist inside offering with id:{offeringId}"));
            }

            else
            {
                Department department = await db.department.Where(dep => dep.departmentId == id).FirstOrDefaultAsync();
                if (name != null) department.departmentName = name;
                db.department.Update(department);
                await db.SaveChangesAsync();
                OnUpdated(new DepartmentArgs("200"));
            }
        }

        public async Task Delete(DepartmentHandler handler, ApplicationContext db, string name, int offeringId)
        {
            deleteDepartment = handler;
            Department department = await db.department.Where(dep => dep.departmentName == name && dep.offeringId == offeringId).FirstOrDefaultAsync();
            if (department != null)
            {
                db.department.Remove(department);
                await db.SaveChangesAsync();
                OnDeleted(new DepartmentArgs("200"));
            }
            else
            {
                OnDeleted(new DepartmentArgs($"There is no department with name {name} and offering {offeringId}"));
            }
        }

        public async Task<List<Department>> Get(ApplicationContext db, int offeringid)
        {
            List<Department> department = await db.department.Where(dep => dep.offeringId == offeringId)
                                                             .ToListAsync();
            return department;
        }
    }
}
