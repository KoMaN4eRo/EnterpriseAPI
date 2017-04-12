using EnterpriseAPI.Models.DepartmentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseAPI.Models.OfferingModel
{
    public class Offering
    {
        [Key]
        public int offeringId { get; set; }
        [Required]
        public string offeringName { get; set; }
        [Required]
        public int familyId { get; set; }
        public List<Department> department { get; set; }

        public Offering() { }
    }
}
