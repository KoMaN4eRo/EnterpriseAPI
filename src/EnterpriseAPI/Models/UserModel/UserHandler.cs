using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.UserModel
{
    public delegate void UserHandler(object sender, UserArgs args);

    public class UserArgs
    {
        public string message { get; private set; }
        public UserArgs(string message)
        {
            this.message = message;
        }
    }
}
