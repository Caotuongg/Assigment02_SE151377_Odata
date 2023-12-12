using Assigment02_BussinessObject;
using Assigment02_Repositories;
using Assigment02_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<User> GetUser(int id)
        {
            try
            {
                var check = await userRepository.Get(id);
                if (check != null)
                {
                    return check;
                }
                else
                {
                    throw new Exception("Not Found User");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Add(User user)
        {
            try
            {
                return await userRepository.Add(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(User user)
        {
            try
            {
                var check = await userRepository.Get(user.user_id);
                if (check != null)
                {
                    check.user_id = user.user_id;
                    check.email_address = user.email_address;
                    check.password = user.password;
                    check.source = user.source;
                    check.first_name = user.first_name;
                    check.middle_name = user.middle_name;
                    check.last_name = user.last_name;
                    check.hire_date = user.hire_date;
                    return await userRepository.Update(user.user_id, check);

                }
                else
                {
                    throw new Exception("Not Found User");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var mem = await userRepository.Get(id);
                if (mem != null)
                {
                    return await userRepository.Delete(id);

                }
                else
                {
                    throw new Exception("Not Found User");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User> Login(string email, string password)
        {

            try
            {
                var account =  userRepository.GetAll();
                return account.SingleOrDefault(x => x.email_address == email && x.password == password);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
