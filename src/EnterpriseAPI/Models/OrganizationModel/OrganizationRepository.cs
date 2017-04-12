using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public class OrganizationRepository : IOrganizationRepository
    {
        public async Task Create(ApplicationContext db, Organization org)
        {
            db.organization.Add(
                new Organization()
                {
                    organizationName = org.organizationName,
                    organizationCode = org.organizationCode,
                    organizationType = org.organizationType,
                    Owner = org.Owner
                });
            await db.SaveChangesAsync();
        }

        public async Task Update(ApplicationContext db, int id, string name = null, int code = 0, string type = null)
        {
            Organization org = await db.organization.Where(o => o.organizationId == id).FirstOrDefaultAsync();
            if (name != null) org.organizationName = name;
            if (code != 0) org.organizationCode = code;
            if (type != null) org.organizationType = type;
            db.organization.Update(org);
            await db.SaveChangesAsync();
        }

        public async Task Delete(ApplicationContext db, string name)
        {
            Organization org = await db.organization.Where(o => o.organizationName == name).FirstOrDefaultAsync();
            db.organization.Remove(org);
            await db.SaveChangesAsync();            
        }

        public async Task<Organization> ExpandAll(ApplicationContext db, int id)
        {
            Organization organization = await db.organization.Where(o => o.organizationId == id).FirstOrDefaultAsync();
            organization = await db.organization
                                            .Include(o =>o.country)
                                            .Where(o => o.organizationId == id)
                                            .FirstOrDefaultAsync();
                foreach (Country c in organization.country)
                {
                    Country countr = await db.country
                             .Include(p => p.business)
                             .Where(p => p.countryId == c.countryId)
                             .FirstOrDefaultAsync();
                    c.business = countr.business;
                    if (c.business == null) return organization;
                    foreach (Business b in c.business)
                    {
                        Business business = await db.business
                             .Include(p => p.family)
                             .Where(p => p.businessId == b.businessId)
                             .FirstOrDefaultAsync();
                        b.family = business.family;
                        if (b.family == null) return organization;
                        foreach (Family f in b.family)
                        {
                            Family family = await db.family
                                 .Include(p => p.offering)
                                 .Where(p => p.familyId == f.familyId)
                                 .FirstOrDefaultAsync();
                            f.offering = family.offering;
                            if (f.offering == null) return organization;
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
            return organization;
        }

        public async Task<List<Organization>> GetCurrentOwnerOrganization(ApplicationContext db, string owner)
        {
            List<Organization> organization = await db.organization.Where(o => o.Owner.Equals(owner)).ToListAsync();
            return organization;
        }

        public async Task<List<Organization>> Get(ApplicationContext db)
        {
            List<Organization> organization = await db.organization.ToListAsync();
            return organization;
        }

        public async Task<List<Organization>> GetByType(ApplicationContext db, string organizationType)
        {
            List<Organization> organization = await db.organization.Where(o => o.organizationType.Equals(organizationType))
                                                                   .ToListAsync();
            return organization;
        }
    }
}
