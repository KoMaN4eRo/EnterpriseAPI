using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.OrganizationModel;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using EnterpriseAPI.Models.DepartmentModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using EnterpriseAPI.Models.UserModel;

namespace EnterpriseAPI.Models
{
    public class ApplicationContext: DbContext
    {
        private DbContextOptionsBuilder optionsBuilder;

        public DbSet<Organization> organization { get; set; }
        public DbSet<Business> business { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<Family> family { get; set; }
        public DbSet<Offering> offering { get; set; }
        public DbSet<Department> department { get; set; }
        public DbSet<User> user { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }
    }
}
