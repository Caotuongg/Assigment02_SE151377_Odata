using Assigment02_BussinessObject;
using Assigment02_Repositories;
using Assigment02_Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Service
{
    public class BookService: IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBookRepository bookRepository;
        private readonly IPublisherRepository publisherRepository;
        public BookService(IUnitOfWork unitOfWork, IBookRepository bookRepository, IPublisherRepository publisherRepository)
        {
            this.unitOfWork = unitOfWork;
            this.bookRepository = bookRepository;
            this.publisherRepository = publisherRepository;
        }

        public async Task<Book> GetBookById(int id)
        {
            try
            {
                var check = await unitOfWork.BookRepository.Get(id);
                if (check != null)
                {
                   check.Publisher = await unitOfWork.PublisherRepository.Get(check.pub_id);
                   return check;
                }
                else
                {
                    throw new Exception("Not Found Book");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<Book> GetAll()
        {
            try
            {
                var books = bookRepository.GetAll();
                foreach (var rating in books)
                {
                    rating.Publisher = publisherRepository.Get(rating.pub_id).Result;
                    rating.Publisher.Books = null;
                }

                return books;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<Book>> GetBooks(string search)
        {
            try
            {
                var products = new List<Book>();
                if (string.IsNullOrWhiteSpace(search))
                {
                    var uow = await unitOfWork.BookRepository.GetAllBook();
                    products = uow.ToList();
                }
                else
                {
                    var repo = await unitOfWork.BookRepository.GetAllBook(search.Trim());
                    products = repo.ToList();
                }
                foreach (var product in products)
                {
                    product.Publisher = await unitOfWork.PublisherRepository.Get(product.pub_id);
                    
                }
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Add(Book book)
        {
            try
            {
                return await unitOfWork.BookRepository.Add(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Update(Book book)
        {
            try
            {
                var check = await unitOfWork.BookRepository.Get(book.book_id);
                if (check != null)
                {
                    check.book_id = book.book_id;
                    check.title = book.title;
                    check.type = book.type;
                    check.price = book.price;
                    check.advance = book.advance;
                    check.pub_id = book.pub_id;
                    check.royalty = book.royalty;
                    check.ytd_sales = book.ytd_sales;
                    check.notes = book.notes;
                    check.published_date = book.published_date;
                    return await unitOfWork.BookRepository.Update(book.book_id, check);

                }
                else
                {
                    throw new Exception("Not Found Book");
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
                var mem = await unitOfWork.BookRepository.Get(id);
                if (mem != null)
                {
                    return await unitOfWork.BookRepository.Delete(id);

                }
                else
                {
                    throw new Exception("Not Found Book");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}

