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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository authorRepository;  
        public AuthorService(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<Author> GetAuthor(int id)
        {
            try
            {
                var check = await authorRepository.Get(id);
                if (check != null)
                {
                    return check;
                }
                else
                {
                    throw new Exception("Not Found Author");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public IEnumerable<Author> GetAll()
        {
            try
            {
                return authorRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Add(Author author)
        {
            try
            {
                return await authorRepository.Add(author);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(Author author)
        {
            try
            {
                var check = await authorRepository.Get(author.author_id);
                if (check != null)
                {
                    
                        check.last_name = author.last_name;
                        check.first_name = author.first_name;
                        check.email_address = author.email_address;
                        check.phone = author.phone;
                        check.address = author.address;
                        check.city = author.city;
                        check.state = author.state;
                        check.zip = author.zip;
                        return await authorRepository.Update(author.author_id, check);
                    
                }
                else
                {
                    throw new Exception("Not Found Author");
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
                var mem = await authorRepository.Get(id);
                if (mem != null)
                {
                    return await authorRepository.Delete(id);
                    
                }
                else
                {
                    throw new Exception("Not Found Author");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
    }
}
