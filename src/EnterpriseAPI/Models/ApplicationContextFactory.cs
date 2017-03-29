using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models
{
    public class ApplicationContextFactory : IDbContextFactory<ApplicationContext>
    {
        public static ApplicationContext Create()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EnterpriseAPIdb;Trusted_Connection=True;");

            return new ApplicationContext(optionsBuilder.Options);
        }

        public ApplicationContext Create(DbContextFactoryOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EnterpriseAPIdb;Trusted_Connection=True;");

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
