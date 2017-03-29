using EnterpriseAPI.Models.DepartmentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OfferingModel
{
    public class Offering : IOffering
    {
        [Key]
        public int offeringId { get; set; }
        [Required]
        public string offeringName { get; set; }
        [Required]
        public int familyId { get; set; }
        public List<Department> department { get; set; }

        protected internal event OfferingHandler createOffering;
        protected internal event OfferingHandler updateOffering;
        protected internal event OfferingHandler deleteOffering;

        public Offering() { }

        private void CallEvent(OfferingArgs e, OfferingHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(OfferingArgs e)
        {
            CallEvent(e, createOffering);
        }

        protected virtual void OnUpdated(OfferingArgs e)
        {
            CallEvent(e, updateOffering);
        }

        protected virtual void OnDeleted(OfferingArgs e)
        {
            CallEvent(e, deleteOffering);
        }

        public async Task Create(OfferingHandler handler, ApplicationContext db, string name, int familyId)
        {
            createOffering = handler;
            if (await db.offering.AnyAsync(f => f.offeringName == name && f.familyId == familyId))
            {
                OnCreated(new OfferingArgs($"Offering with name:{name} already exist inside family with id:{familyId}"));
            }

            else
            {
                db.offering.Add(new Offering() { offeringName = name, familyId = familyId});
                await db.SaveChangesAsync();
                OnCreated(new OfferingArgs("200"));
            }
        }

        public async Task Update(OfferingHandler handler, ApplicationContext db, int familyId, int id, string name)
        {
            updateOffering = handler;
            if (await db.offering.AnyAsync(c => c.offeringName == name && c.familyId == familyId))
            {
                OnUpdated(new OfferingArgs($"Offering with name:{name} already exist inside family with id:{familyId}"));
            }

            else
            {
                Offering offering = await db.offering.Where(f => f.offeringId == id).FirstOrDefaultAsync();
                if (name != null) offering.offeringName = name;
                db.offering.Update(offering);
                await db.SaveChangesAsync();
                OnUpdated(new OfferingArgs("200"));
            }
        }

        public async Task Delete(OfferingHandler handler, ApplicationContext db, string name, int familyId)
        {
            deleteOffering = handler;
            Offering offering = await db.offering.Where(off => off.offeringName == name && off.familyId == familyId).FirstOrDefaultAsync();
            if (offering != null)
            {
                db.offering.Remove(offering);
                await db.SaveChangesAsync();
                OnDeleted(new OfferingArgs("200"));
            }
            else
            {
                OnDeleted(new OfferingArgs($"There is no offering with name {name} and familyId {familyId}"));
            }
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
