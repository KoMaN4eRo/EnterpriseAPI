using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OfferingModel
{
    public class OfferingRepository : IOfferingRepository
    {
        public async Task Create(ApplicationContext db, Offering offering)
        {
            db.offering.Add(offering);
            await db.SaveChangesAsync();
        }

        public async Task Update(ApplicationContext db, int familyId, int id, string name)
        {
            Offering offering = await db.offering.Where(f => f.offeringId == id).FirstOrDefaultAsync();
            if (name != null) offering.offeringName = name;
            db.offering.Update(offering);
            await db.SaveChangesAsync();
        }

        public async Task Delete(ApplicationContext db, string name, int familyId)
        {
            Offering offering = await db.offering.Where(off => off.offeringName == name && off.familyId == familyId).FirstOrDefaultAsync();
            db.offering.Remove(offering);
            await db.SaveChangesAsync();
        }

        public async Task<List<Offering>> ExpandAll(ApplicationContext db, int familyId)
        {
            List<Offering> offeringList = await db.offering.Where(off => off.familyId == familyId)
                                                       .ToListAsync();
            foreach (Offering off in offeringList)
            {
                Offering offering = await db.offering
                     .Include(p => p.department)
                     .Where(p => p.offeringId == off.offeringId)
                     .FirstOrDefaultAsync();
                off.department = offering.department;
            }
            return offeringList;
        }

        public async Task<List<Offering>> Get(ApplicationContext db, int familyId)
        {
            List<Offering> offeringList = await db.offering.Where(off => off.familyId == familyId)
                                                       .ToListAsync();
            return offeringList;
        }
    }
}
