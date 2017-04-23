using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Validation.ValidationAttributes
{
    public class MinusCodeAttribute:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if ((int)value < 0)
                return false;
            return true;
        }
    }
}
