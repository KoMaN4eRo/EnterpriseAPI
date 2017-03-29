using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.CountryModel
{
    public delegate void CountryHandler(object sender, CountryArgs args);

    public class CountryArgs
    {
        public string message { get; private set; }
        public CountryArgs(string message)
        {
            this.message = message;
        }
    }
}
