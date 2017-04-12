using EnterpriseAPI.Models.OfferingModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseAPI.Models.FamilyModel
{
    public class Family
    {
        [Key]
        public int familyId { get; set; }
        [Required]
        public string familyName { get; set; }
        [Required]
        public int businessId { get; set; }
        public List<Offering> offering { get; set; }

        public Family() { }
    }
}
