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
    public class Country
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

        public Country() {}
    }
}
