using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.CountryModel
{
    public class CountryRepository : ICountryRepository
    {
        public async Task Create(ApplicationContext db, Country country)
        {
            db.country.Add(new Country()
            {
                countryName = country.countryName,
                countryCode = country.countryCode,
                organizationId = country.organizationId
            });
            await db.SaveChangesAsync();
        }

        public async Task Update(ApplicationContext db, int organizationId, int id, string name = null, int code = 0)
        {
            Country country = await db.country.Where(c => c.countryId == id).FirstOrDefaultAsync();
            if (name != null) country.countryName = name;
            if (code != 0) country.countryCode = code;
            db.country.Update(country);
            await db.SaveChangesAsync();
        }

        public async Task Delete(ApplicationContext db, string name, int organizationId)
        {
            Country country = await db.country.Where(c => c.countryName == name && c.organizationId == organizationId).FirstOrDefaultAsync();
            db.country.Remove(country);
            await db.SaveChangesAsync();
        }

        public async Task<List<Country>> ExpandAll(ApplicationContext db, int organizationId)
        {
            List<Country> country = await db.country.Where(o => o.organizationId == organizationId)
                                                   .ToListAsync();
            foreach (Country c in country)
            {
                Country countr = await db.country
                         .Include(p => p.business)
                         .Where(p => p.countryId == c.countryId)
                         .FirstOrDefaultAsync();
                c.business = countr.business;
                foreach (Business b in c.business)
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
            }
            return country;
        }

        public async Task<List<Country>> Get(ApplicationContext db, int organizationId)
        {
            List<Country> country = await db.country.Where(o => o.organizationId == organizationId)
                                                    .ToListAsync();
            return country;
        }
    }
}
