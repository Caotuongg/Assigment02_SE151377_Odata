using Assigment02_BussinessObject;
using Assigment02_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<User> _userRepository;

        private IGenericRepository<Author> _authorRepository;

        private IBookRepository _bookRepository;

        private IBookAuthorRepository _bookAuthorRepository;

        private IPublisherRepository _publisherRepository;
        private IRoleRepository _roleRepository;

        private readonly eBookStoreDBContext context;
        public UnitOfWork(eBookStoreDBContext context)
        {
            if (this.context == null)
            {
                this.context = context;
            }
            _userRepository = new GenericRepository<User>(context);
            _authorRepository = new GenericRepository<Author>(context);
            _bookRepository = new BookRepository(context);
            _bookAuthorRepository = new BookAuthorRepository(context);
            _publisherRepository = new PublisherRepository(context);
            _roleRepository = new RoleRepository(context);
        }
        public IGenericRepository<User> UserRepository
        {
            get
            {
                return _userRepository;
            }
        }

        public IGenericRepository<Author> AuthorRepository
        {
            get
            {
                return _authorRepository;
            }
        }

        public IBookRepository BookRepository
        {
            get
            {
                return _bookRepository;
            }
        }

        public IBookAuthorRepository BookAuthorRepository
        {
            get
            {
                return _bookAuthorRepository;
            }
        }
        public IPublisherRepository PublisherRepository
        {
            get
            {
                return _publisherRepository;
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository;
            }
        }


        public void SaveChange()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
