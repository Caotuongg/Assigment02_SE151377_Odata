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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly eBookStoreDBContext eBookStoreDBContext;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(eBookStoreDBContext eBookStoreDBContext)
        {
            if (this.eBookStoreDBContext == null)
            {
                this.eBookStoreDBContext = eBookStoreDBContext;
            }
            dbSet = this.eBookStoreDBContext.Set<T>();
        }
        public async Task<bool> Add(T item)
        {
            try
            {
                dbSet.Add(item);
                await eBookStoreDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(object id)
        {
            try
            {
                var item = dbSet.Find(id);
                if (item != null)
                {
                    dbSet.Remove(item);
                    await eBookStoreDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<T> Get(object id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {
                IQueryable<T> query = dbSet;
                var result = query.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(object id, T item)
        {
            try
            {
                var check = dbSet.Find(id);
                if (check != null)
                {
                    eBookStoreDBContext.Entry(check).State = EntityState.Detached;
                    dbSet.Update(item);
                    await eBookStoreDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual async Task<IEnumerable<T>> GetAllBook()
        {
            try
            {
                return await dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error when get {nameof(T)}: {ex.Message}");
            }
        }
    }
}
