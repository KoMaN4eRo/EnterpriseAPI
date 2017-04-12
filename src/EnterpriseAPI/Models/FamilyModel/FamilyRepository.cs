using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.FamilyModel
{
    public class FamilyRepository : IFamilyRepository
    {
        public async Task Create(ApplicationContext db, Family family)
        {
            db.family.Add(family);
            await db.SaveChangesAsync();
        }


        public async Task Update(ApplicationContext db, int businessId, int id, string name)
        {
            Family family = await db.family.Where(f => f.familyId == id).FirstOrDefaultAsync();
            if (name != null)
            family.familyName = name;
            db.family.Update(family);
            await db.SaveChangesAsync();
        }

        public async Task Delete(ApplicationContext db, string name, int businessId)
        {
            Family family = await db.family.Where(f => f.familyName == name && f.businessId == businessId).FirstOrDefaultAsync();
            db.family.Remove(family);
            await db.SaveChangesAsync();
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
