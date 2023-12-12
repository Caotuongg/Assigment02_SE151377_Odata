using Assigment02_BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public interface IPublisherService
    {
        Task<Publisher> GetPublisher(int id);
        IEnumerable<Publisher> GetAll();
        Task<bool> Add(Publisher publisher);
        Task<bool> Update(Publisher publisher);
        Task<bool> Delete(int id);
    }
}
