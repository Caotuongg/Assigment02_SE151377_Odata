using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public interface IUserService
    {
        Task<User> GetUser(int id);
        IEnumerable<User> GetAll();
        Task<bool> Add(User user);
        Task<bool> Update(User user);
        Task<bool> Delete(int id);
        Task<User> Login(string email, string password);
    }
}
