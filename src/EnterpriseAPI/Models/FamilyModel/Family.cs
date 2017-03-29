using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.FamilyModel
{
    public class Family: IFamily
    {
        [Key]
        public int familyId { get; set; }
        [Required]
        public string familyName { get; set; }
        [Required]
        public int businessId { get; set; }
        public List<Offering> offering { get; set; }

        protected internal event FamilyHandler createFamily;
        protected internal event FamilyHandler updateFamily;
        protected internal event FamilyHandler deleteFamily;

        public Family() { }
        
        private void CallEvent(FamilyArgs e, FamilyHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(FamilyArgs e)
        {
            CallEvent(e, createFamily);
        }

        protected virtual void OnUpdated(FamilyArgs e)
        {
            CallEvent(e, updateFamily);
        }

        protected virtual void OnDeleted(FamilyArgs e)
        {
            CallEvent(e, deleteFamily);
        }
        
        public async Task Create(FamilyHandler handler, ApplicationContext db, string name, int businessId)
        {
            createFamily = handler;
            if (await db.family.AnyAsync(f => f.familyName == name && f.businessId == businessId))
            {
                OnCreated(new FamilyArgs($"Family with name:{name} already exist inside business with id:{businessId}"));
            }

            else
            {
                db.family.Add(new Family() { familyName = name, businessId = businessId});
                await db.SaveChangesAsync();
                OnCreated(new FamilyArgs("200"));
            }
        }

        public async Task Update(FamilyHandler handler, ApplicationContext db, int businessId, int id, string name)
        {
            updateFamily = handler;
            if (await db.family.AnyAsync(c => c.familyName == name && c.businessId == businessId))
            {
                OnUpdated(new FamilyArgs($"Family with name:{name} already exist inside business with id:{businessId}"));
            }

            else
            {
                Family family = await db.family.Where(f => f.familyId == id).FirstOrDefaultAsync();
                if (name != null) family.familyName = name;
                db.family.Update(family);
                await db.SaveChangesAsync();
                OnUpdated(new FamilyArgs("200"));
            }
        }

        public async Task Delete(FamilyHandler handler, ApplicationContext db, string name, int businessId)
        {
            deleteFamily = handler;
            Family family = await db.family.Where(f => f.familyName == name && f.familyId == businessId).FirstOrDefaultAsync();
            if (family != null)
            {
                db.family.Remove(family);
                await db.SaveChangesAsync();
                OnDeleted(new FamilyArgs("200"));
            }
            else
            {
                OnDeleted(new FamilyArgs($"There is no family with name {name} and businessId {businessId}"));
            }
        }

        public async Task<List<Family>> ExpandAll(ApplicationContext db, int businessId)
        {
            List<Family> familyList = await db.family.Where(f => f.businessId == businessId)
                                                     .ToListAsync();
            foreach (Family f in familyList)
            {
                Family family = await db.family
                     .Include(p => p.offering)
                     .Where(p => p.familyId == f.familyId)
                     .FirstOrDefaultAsync();
                f.offering = family.offering;
                foreach (Offering off in f.offering)
                {
                    Offering offering = await db.offering
                         .Include(p => p.department)
                         .Where(p => p.offeringId == off.offeringId)
                         .FirstOrDefaultAsync();
                    off.department = offering.department;
                }
            }
            return familyList;
        }

        public async Task<List<Family>> Get(ApplicationContext db, int businessId)
        {
            List<Family> familyList = await db.family.Where(f => f.businessId == businessId)
                                                     .ToListAsync();
            return familyList;
        }
    }
}
