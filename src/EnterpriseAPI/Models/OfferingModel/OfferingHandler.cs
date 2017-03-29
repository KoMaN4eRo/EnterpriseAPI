using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.OfferingModel
{
    public delegate void OfferingHandler(object sender, OfferingArgs args);

    public class OfferingArgs
    {
        public string message { get; private set; }
        public OfferingArgs(string message)
        {
            this.message = message;
        }
    }
}
