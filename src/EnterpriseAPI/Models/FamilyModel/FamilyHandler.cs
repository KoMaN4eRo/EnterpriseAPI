using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.FamilyModel
{
    public delegate void FamilyHandler(object sender, FamilyArgs args);

    public class FamilyArgs
    {
        public string message { get; private set; }
        public FamilyArgs(string message)
        {
            this.message = message;
        }
    }
}
