using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public class Business
    {
        [Key]
        public int businessId { get; set; }
        [Required]
        public string businessName { get; set; }
        [Required]
        public int countryId { get; set; }
        public List<Family> family { get; set; }

        public Business() { }
    }
}
