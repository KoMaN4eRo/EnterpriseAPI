using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OrganizationModel
{
    public delegate void OrganizationHandler(object sender, OrganizationArgs args);
    
    public class OrganizationArgs
    {
        public string message { get; set; }
        public OrganizationArgs(string message)
        {
            this.message = message;
        }
    }
}
