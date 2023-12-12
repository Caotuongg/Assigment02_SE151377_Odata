using Assigment02_BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public interface IAuthorService
    {
        Task<Author> GetAuthor(int id);
        IEnumerable<Author> GetAll();
        Task<bool> Add(Author author);
        Task<bool> Update(Author author);
        Task<bool> Delete(int id);
    }
}
