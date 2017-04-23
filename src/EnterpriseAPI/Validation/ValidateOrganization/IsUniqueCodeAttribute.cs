using EnterpriseAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Validation.ValidateOrganization
{
    public class IsUniqueCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            //if (db.organization.FirstOrDefault(o => o.organizationCode == value) == null)
            // return true;
            //else return false;
            return true;    
        }
    }
}
