using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models.DepartmentModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public class Organization: IOrganization
    {
        [Key]
        public int organizationId { get; set; }
        [Required]
        public string organizationName { get; set; }
        [Required]
        public int organizationCode { get; set; }
        [Required]
        public string organizationType { get; set; }
        public string Owner { get; set; }
        public List<Country> country { get; set; }

        protected internal event OrganizationHandler createOrganization;
        protected internal event OrganizationHandler updateOrganization;
        protected internal event OrganizationHandler deleteOrganization;
        
        public Organization()
        {
        }

        private void CallEvent(OrganizationArgs e, OrganizationHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(OrganizationArgs e)
        {
            CallEvent(e, createOrganization);
        }

        protected virtual void OnUpdated(OrganizationArgs e)
        {
            CallEvent(e, updateOrganization);
        }

        protected virtual void OnDeleted(OrganizationArgs e)
        {
            CallEvent(e, deleteOrganization);
        }

        // add new organization 
        public async Task Create(OrganizationHandler create, ApplicationContext db, string name, int code, string type, string owner)
        {
            createOrganization = create;
            
            if (await db.organization.AnyAsync(org => org.organizationName == name))
            {
                OnCreated(new OrganizationArgs($"Organization with name:{name} already exist"));
            }

            else if (await db.organization.AnyAsync(org => org.organizationCode == code))
            {
                OnCreated(new OrganizationArgs($"Organization with code:{code} already exist"));
            }

            else
            {
                db.organization.Add(new Organization() { organizationName = name, organizationCode = code, organizationType = type, Owner = owner });
                await db.SaveChangesAsync();
                OnCreated(new OrganizationArgs("200"));
            }
        }

        // update organization
        public async Task Update(OrganizationHandler update, ApplicationContext db, int id, string name = null, int code = 0, string type = null)
        {
            updateOrganization = update;
            if (await db.organization.AnyAsync(org => org.organizationName == name))
            {
                OnUpdated(new OrganizationArgs($"Organization with name:{name} already exist"));
            }

            else if (await db.organization.AnyAsync(org => org.organizationCode == code))
            {
                OnUpdated(new OrganizationArgs($"Organization with code:{code} already exist"));
            }
            
            else
            {
                Organization org = await db.organization.Where(o => o.organizationId == id).FirstOrDefaultAsync();
                if (name != null) org.organizationName = name;
                if (code != 0)    org.organizationCode = code;
                if (type != null) org.organizationType = type;

                db.organization.Update(org);
                await db.SaveChangesAsync();
                OnUpdated(new OrganizationArgs("200"));
            }
        }

        // delete organization
        public async Task Delete(OrganizationHandler delete, ApplicationContext db, string name)
        {
            deleteOrganization = delete;
            Organization org = await db.organization.Where(o => o.organizationName == name).FirstOrDefaultAsync();
            if (org != null)
            {
                db.organization.Remove(org);
                await db.SaveChangesAsync();
                OnDeleted(new OrganizationArgs("200"));
            }

            else
            {
                OnDeleted(new OrganizationArgs($"There is no organization with name {name}"));
            }
        }
        // Expand all levels from  current
        public async Task<List<Organization>> ExpandAll(ApplicationContext db)
        {
            List<Organization> organization = await db.organization.ToListAsync();
            //using of "Eager loading"
            foreach (var o in organization)
            {
                Organization org1 = await db.organization
                             .Include(p => p.country)
                             .Where(p => p.organizationId == o.organizationId)
                             .FirstOrDefaultAsync();
                o.country = org1.country;
                foreach (Country c in o.country)
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
            }
            return organization;
        }
        
        public async Task<List<Organization>> Get(ApplicationContext db)
        {
            List<Organization> organization = await db.organization.ToListAsync();
            return organization;
        }
        //get from database organizations with concrete organizationType 
        public async Task<List<Organization>> GetByType(ApplicationContext db, string organizationType)
        {
            List<Organization> organization = await db.organization.Where(o => o.organizationType.Equals(organizationType))
                                                                   .ToListAsync();
            return organization;
        }
    }
}
