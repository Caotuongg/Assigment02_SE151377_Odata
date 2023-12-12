using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository publisherRepository;
        public PublisherService(IPublisherRepository publisherRepository)
        {
            this.publisherRepository = publisherRepository;
        }

        public async Task<Publisher> GetPublisher(int id)
        {
            try
            {
                var check = await publisherRepository.Get(id);
                if (check != null)
                {
                    return check;
                }
                else
                {
                    throw new Exception("Not Found Publisher");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Publisher> GetAll()
        {
            try
            {
                return publisherRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Add(Publisher publisher)
        {
            try
            {
                return await publisherRepository.Add(publisher);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(Publisher publisher)
        {
            try
            {
                var check = await publisherRepository.Get(publisher.pub_id);
                if (check != null)
                {

                    check.publisher_name = publisher.publisher_name;
                    check.city = publisher.city;
                    check.state = publisher.state;
                    check.country = publisher.country;
                    
                    return await publisherRepository.Update(publisher.pub_id, check);

                }
                else
                {
                    throw new Exception("Not Found Publisher");
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
                var mem = await publisherRepository.Get(id);
                if (mem != null)
                {
                    return await publisherRepository.Delete(id);

                }
                else
                {
                    throw new Exception("Not Found Publisher");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

