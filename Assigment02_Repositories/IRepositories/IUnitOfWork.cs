using Assigment02_BussinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<Author> AuthorRepository { get; }
        IBookRepository BookRepository { get; }
        IBookAuthorRepository BookAuthorRepository{ get; }
        IPublisherRepository PublisherRepository { get; }
        IRoleRepository RoleRepository { get; }
        void SaveChange();
    }
}
