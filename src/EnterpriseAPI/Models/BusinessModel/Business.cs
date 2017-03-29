using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public class Business : IBusiness
    {
        [Key]
        public int businessId { get; set; }
        [Required]
        public string businessName { get; set; }
        [Required]
        public int countryId { get; set; }
        public List<Family> family { get; set; }

        protected internal event BusinessHandler createBusiness;
        protected internal event BusinessHandler updateBusiness;
        protected internal event BusinessHandler deleteBusiness;

        public Business() { }

        private void CallEvent(BusinessArgs e, BusinessHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(BusinessArgs e)
        {
            CallEvent(e, createBusiness);
        }

        protected virtual void OnUpdated(BusinessArgs e)
        {
            CallEvent(e, updateBusiness);
        }

        protected virtual void OnDeleted(BusinessArgs e)
        {
            CallEvent(e, deleteBusiness);
        }

        public async Task Create(BusinessHandler createBusinessHandler, ApplicationContext db, string name, int countryId)
        {
            createBusiness = createBusinessHandler;
            if (await db.business.AnyAsync(b => b.businessName == name && b.countryId == countryId))
            {
                OnCreated(new BusinessArgs($"Business with name:{name} already exist inside country with id:{countryId}"));
            }
            
            else
            {
                db.business.Add(new Business() { businessName = name, countryId = countryId });
                await db.SaveChangesAsync();
                OnCreated(new BusinessArgs("200"));
            }
        }

        public async Task Update(BusinessHandler updateBusinessHandler, ApplicationContext db, int countryId, int id, string name)
        {
            updateBusiness = updateBusinessHandler;
            if (await db.business.AnyAsync(c => c.businessName == name && c.countryId == countryId))
            {
                OnUpdated(new BusinessArgs($"Business with name:{name} already exist inside country with id:{countryId}"));
            }

            else
            {
                Business business = await db.business.Where(o => o.businessId == id).FirstOrDefaultAsync();
                if (name != null) business.businessName = name;
                db.business.Update(business);
                await db.SaveChangesAsync();
                OnUpdated(new BusinessArgs("200"));
            }
        }

        public async Task Delete(BusinessHandler deleteBusinessHandler,ApplicationContext db, string name, int countryId)
        {
            deleteBusiness = deleteBusinessHandler;
            Business business = await db.business.Where(c => c.businessName == name && c.countryId == countryId).FirstOrDefaultAsync();
            if (business != null)
            {
                db.business.Remove(business);
                await db.SaveChangesAsync();
                OnDeleted(new BusinessArgs("200"));
            }
            else
            {
                OnDeleted(new BusinessArgs($"There is no business with name {name} and countryId {countryId}"));
            }
        }

        // Expand all levels from  current
        public async Task<List<Business>> ExpandAll(ApplicationContext db, int countryId)
        {
            List<Business> businessList = await db.business.Where(b => b.countryId == countryId)
                                                           .ToListAsync();
            foreach (Business b in businessList)
            {
                Business business = await db.business
                     .Include(p => p.family)
                     .Where(p => p.businessId == b.businessId)
                     .FirstOrDefaultAsync();
                b.family = business.family;
                foreach (Family f in b.family)
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
            }
            return businessList;
        }

        // Get only business inside country with Id = countryId
        public async Task<List<Business>> Get(ApplicationContext db, int countryId)
        {
            List<Business> businessList = await db.business.Where(b => b.countryId == countryId)
                                                           .ToListAsync();
            return businessList;
        }
    }
}
