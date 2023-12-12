using Assigment02_BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public interface IBookService
    {
        Task<Book> GetBookById(int id);
        IEnumerable<Book> GetAll();
        Task<bool> Add(Book book);
        Task<bool> Update(Book book);
        Task<bool> Delete(int id);
        Task<IEnumerable<Book>> GetBooks(string search);
    }
}
