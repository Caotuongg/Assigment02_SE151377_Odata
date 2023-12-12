using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> Get(object id);
        Task<bool> Add(T item);
        Task<bool> Update(object id, T item);
        Task<bool> Delete(object id);

        Task<IEnumerable<T>> GetAllBook();
    }
}
