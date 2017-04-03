using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.UserModel
{
    public interface IUser
    {
        Task Create(UserHandler create, ApplicationContext db, string name, string lastName, string email);
        Task<User> Get(ApplicationContext db, string email);
        Task Update(UserHandler update, ApplicationContext db, int id, string Address);
        Task Delete(UserHandler delete, ApplicationContext db, int id);
    }
}
