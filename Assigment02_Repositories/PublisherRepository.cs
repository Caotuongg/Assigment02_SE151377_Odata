using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Repositories
{
    public class PublisherRepository : GenericRepository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(eBookStoreDBContext eBookStoreDBContext) : base(eBookStoreDBContext) { }
    }
}
