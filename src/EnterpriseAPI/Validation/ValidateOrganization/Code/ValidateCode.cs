using EnterpriseAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Validation.ValidateOrganization.Code
{
    public class ValidateCode: IValidateCode
    {
        private readonly ApplicationContext db;
        public ValidateCode(ApplicationContext _db)
        {
            db = _db;
        }

        public async Task<bool> IsValidCode(int code)
        {
            if (code == 0 || code < 0)
                return true;

            if (await db.organization.FirstOrDefaultAsync(o => o.organizationCode == code) == null)
                return true;

            else
                return false;
        }
    }
}
