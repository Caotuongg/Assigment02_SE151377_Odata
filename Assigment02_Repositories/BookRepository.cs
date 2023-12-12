using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(eBookStoreDBContext dbContext) : base(dbContext) 
        {

        }
        public async Task<IEnumerable<Book>> GetAllBook(string search)
        {
            try
            {
                if (double.TryParse(search, out var result))
                {
                    return await dbSet.Where(x => x.price == result).ToListAsync();
                }
                else
                {
                    return await dbSet.Where(x => x.bookname.ToLower().Contains(search.ToLower())).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
