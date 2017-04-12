using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public class BusinessRepository : IBusinessRepository
    {
        public async Task Create(ApplicationContext db, Business business)
        {
            try
            {
                db.business.Add(
                new Business()
                {
                    businessName = business.businessName,
                    countryId = business.countryId
                }
                );
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Update(ApplicationContext db, int countryId, int id, string name)
        {
            Business business = await db.business.Where(o => o.businessId == id).FirstOrDefaultAsync();
            if (name != null) business.businessName = name;
            db.business.Update(business);
            await db.SaveChangesAsync();
        }

        public async Task Delete(ApplicationContext db, string name, int countryId)
        {
            Business business = await db.business.Where(c => c.businessName == name && c.countryId == countryId).FirstOrDefaultAsync();
            db.business.Remove(business);
            await db.SaveChangesAsync();
        }

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

        public async Task<List<Business>> Get(ApplicationContext db, int countryId)
        {
            List<Business> businessList = await db.business.Where(b => b.countryId == countryId)
                                                           .ToListAsync();
            return businessList;
        }
    }
}
