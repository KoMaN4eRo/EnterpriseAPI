using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.DepartmentModel
{
    public delegate void DepartmentHandler(object sender, DepartmentArgs args);

    public class DepartmentArgs
    {
        public string message { get; private set; }
        public DepartmentArgs(string message)
        {
            this.message = message;
        }
    }
}
