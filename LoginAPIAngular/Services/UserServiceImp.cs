using LoginAPIAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPIAngular.Services
{
    public class UserServiceImp : IUserService
    {
        private readonly ApplicationContext _context;
        public UserServiceImp(ApplicationContext context) {
            this._context = context;
        }
        void IUserService.addUser(User user)
        {
            this._context.Users.Add(user);
            this._context.SaveChanges();
        }

        List<User> IUserService.allUsers()
        {
            return this._context.Users.ToList();
        }

        void IUserService.fillData()
        {
            if (this._context.Users.ToList().Count == 0)
            {
                this._context.Users.Add(new User { Username = "Safae", Password = "yes please", Role = "Admin" });
                this._context.Users.Add(new User { Username = "ETOUSY", Password = "123456", Role = "Invtited" });
                this._context.Users.Add(new User { Username = "Tazribine", Password = "123456", Role = "Invtited" });
                this._context.Users.Add(new User { Username = "Ouarachi", Password = "123456", Role = "Manager" });
                this._context.SaveChanges();
            }
        }

        User IUserService.findUser(int id)
        {
            return this._context.Users.Find(id);
        }
    }
}
