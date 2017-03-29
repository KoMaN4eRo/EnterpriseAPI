using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.BusinessModel
{
    public delegate void BusinessHandler(object sender, BusinessArgs args);

    public class BusinessArgs
    {
        public string message { get; private set; }
        public BusinessArgs(string message)
        {
            this.message = message;
        }
    }
}
