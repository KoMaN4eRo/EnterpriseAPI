using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnterpriseAPI.Models.UserModel
{
    public class User:IUser
    {
        [Key]
        public int userId { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string address { get; set; }

        protected internal event UserHandler createUser;
        protected internal event UserHandler updateUser;
        protected internal event UserHandler deleteUser;

        public User()
        {
        }

        private void CallEvent(UserArgs e, UserHandler handler)
        {
            if (handler != null && e != null)
                handler(this, e);
        }

        protected virtual void OnCreated(UserArgs e)
        {
            CallEvent(e, createUser);
        }

        protected virtual void OnUpdated(UserArgs e)
        {
            CallEvent(e, updateUser);
        }

        protected virtual void OnDeleted(UserArgs e)
        {
            CallEvent(e, deleteUser);
        }

        // add new User 
        public async Task Create(UserHandler create, ApplicationContext db, string name, string lastName, string email)
        {
            createUser = create;

            if (await db.user.AnyAsync(u => u.email.Equals(email)))
            {
                OnCreated(new UserArgs($"User {name} {lastName} already exist"));
            }

            else
            {
                db.user.Add(new User() { name = name, lastName = lastName, email = email});
                await db.SaveChangesAsync();
                OnCreated(new UserArgs("200"));
            }
        }

        // update user
        public async Task Update(UserHandler update, ApplicationContext db, int id, string Address)
        {
            updateUser = update;
            if (!(await db.user.AnyAsync(u => u.userId == id)))
            {
                OnCreated(new UserArgs($"There is no user with id {id}"));
            }
            else
            {
                User user = await db.user.Where(u => u.userId == id).FirstOrDefaultAsync();
                user.address = Address;
                db.user.Update(user);
                await db.SaveChangesAsync();
                OnUpdated(new UserArgs("User profile has been updated successfully"));
            }
        }

        // delete user from database
        public async Task Delete(UserHandler delete, ApplicationContext db, int id)
        {
            deleteUser = delete;
            User user = await db.user.Where(o => o.userId == id).FirstOrDefaultAsync();
            if (user != null)
            {
                db.user.Remove(user);
                await db.SaveChangesAsync();
                OnDeleted(new UserArgs(@"User with id:{id} has been successfully deleted"));
            }

            else
            {
                OnDeleted(new UserArgs($"There is no user with id {id}"));
            }
        }
        
        public async Task<User> Get(ApplicationContext db, string email)
        {
            User user = await db.user.Where(u => u.email == email).FirstOrDefaultAsync();
            
            return user;
        }
    }
}
