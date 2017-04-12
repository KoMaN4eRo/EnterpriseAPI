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
    public class Organization
    {
        [Key]
        public int organizationId { get; set; }
        [Required (ErrorMessage = ("OrganizationName isn't indicated"))]
        public string organizationName { get; set; }
        [Required (ErrorMessage = ("OrganizationCode isn't indicated"))]
        public int organizationCode { get; set; }
        [Required(ErrorMessage = ("OrganizationType isn't indicated"))]
        public string organizationType { get; set; }
        public string Owner { get; set; }
        public List<Country> country { get; set; }

        public Organization() { }
    }
}
