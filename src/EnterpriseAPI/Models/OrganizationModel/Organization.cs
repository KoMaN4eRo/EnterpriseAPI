using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models.DepartmentModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using EnterpriseAPI.Validation.ValidateOrganization;
using EnterpriseAPI.Validation.ValidationAttributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public class Organization
    {
        [Key]
        public int organizationId { get; set; }

        [IsNullOrEmpty (ErrorMessage = "Organization name isn't indicated")]
        public string organizationName { get; set; }

        [IsNullOrEmpty(ErrorMessage = "Organization code is 0")]
        [MinusCode(ErrorMessage = "Organization code is less then 0")]
        public int organizationCode { get; set; }

        [IsNullOrEmpty(ErrorMessage = "Organization type isn't indicated")]
        public string organizationType { get; set; }

        public string Owner { get; set; }
        public List<Country> country { get; set; }

        public Organization() { }
    }
}
