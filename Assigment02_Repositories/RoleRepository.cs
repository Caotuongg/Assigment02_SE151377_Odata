using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(eBookStoreDBContext eBookStoreDBContext) : base(eBookStoreDBContext) { }
    }
}
