using LoginAPIAngular.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginAPIAngular.Services
{
    public interface IUserService
    {
        public void fillData();
        public void addUser(User user);
        public User findUser(int id);
        public List<User> allUsers();
    }
}
