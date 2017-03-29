using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.CountryModel
{
    public class Country : ICountry
    {
        [Key]
        public int countryId { get; set; }
        [Required]
        public string countryName { get; set; }
        [Required]
        public int countryCode { get;set; }
        [Required]
        public int organizationId { get; set; }
        public List<Business> business { get; set; }

        protected internal event CountryHandler createCountry;
        protected internal event CountryHandler updateCountry;
        protected internal event CountryHandler deleteCountry;

        public Country() { }

        private void CallEvent(CountryArgs e, CountryHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(CountryArgs e)
        {
            CallEvent(e, createCountry);
        }

        protected virtual void OnUpdated(CountryArgs e)
        {
            CallEvent(e, updateCountry);
        }

        protected virtual void OnDeleted(CountryArgs e)
        {
            CallEvent(e, deleteCountry);
        }

        public async Task Create(CountryHandler create, ApplicationContext db, string name, int code, int organizationId)
        {
            createCountry = create;
            if (await db.country.AnyAsync(cont => cont.countryName == name && cont.organizationId == organizationId))
            {
                OnCreated(new CountryArgs($"Country with name:{name} already exist inside organization with id:{organizationId}"));
            }

            else if (await db.country.AnyAsync(cont => cont.countryCode == code && cont.organizationId == organizationId))
            {
                OnCreated(new CountryArgs($"Country with code:{code} already exist inside organization with id:{organizationId}"));
            }

            else
            {
                db.country.Add(new Country() { countryName = name, countryCode = code, organizationId = organizationId});
                await db.SaveChangesAsync();
                OnCreated(new CountryArgs("200"));
            }
        }

        public async Task Update(CountryHandler update, ApplicationContext db, int organizationId, int id, string name = null, int code = 0)
        {
            updateCountry = update;
            if (await db.country.AnyAsync(c => c.countryName == name && c.organizationId == organizationId))
            {
                OnUpdated(new CountryArgs($"Country with name:{name} already exist inside organization with id:{organizationId}"));
            }

            else if (await db.country.AnyAsync(c => c.countryCode == code && c.organizationId == organizationId))
            {
                OnUpdated(new CountryArgs($"Country with code:{code} already exist inside organization with id:{organizationId}"));
            }

            else
            {
                Country country = await db.country.Where(c => c.countryId == id).FirstOrDefaultAsync();
                if (name != null) country.countryName = name;
                if (code != 0) country.countryCode = code;
                db.country.Update(country);
                await db.SaveChangesAsync();
                OnUpdated(new CountryArgs("200"));
            }
        }

        public async Task Delete(CountryHandler delete,ApplicationContext db, string name, int organizationId)
        {
            deleteCountry = delete;
            Country country = await db.country.Where(c => c.countryName == name && c.organizationId == organizationId).FirstOrDefaultAsync();
            if (country != null)
            {
                db.country.Remove(country);
                await db.SaveChangesAsync();
                OnDeleted(new CountryArgs("200"));
            }
            else
            {
                OnDeleted(new CountryArgs($"There is no country with name {name} and organizationId {organizationId}"));
            }
        }
        // Expand all levels from  current
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

        // Get only country inside organization with Id = organizationId
        public async Task<List<Country>> Get(ApplicationContext db, int organizationId)
        {
            List<Country> country = await db.country.Where(o => o.organizationId == organizationId)
                                                    .ToListAsync();
            return country;
        }
    }
}
