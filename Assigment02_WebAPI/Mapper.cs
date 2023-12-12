using Assigment02_BussinessObject;
using Assigment02_WebAPI.ViewModel;
using AutoMapper;

namespace Assigment02_WebAPI
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Author, AuthorVM>();
            CreateMap<AuthorVM, Author>();
            CreateMap<Author, AuthorUpdateVM>();
            CreateMap<AuthorUpdateVM, Author>();

            CreateMap<Book, BookVM>();
            CreateMap<BookVM, Book>();

            CreateMap<Book, BookUpdateVM>();
            CreateMap<BookUpdateVM, Book>();

            CreateMap<Publisher, PublisherVM>();
            CreateMap<PublisherVM, Publisher>();
            CreateMap<Publisher, PublisherUpdateVM>();
            CreateMap<PublisherUpdateVM, Publisher>();

            CreateMap<User, UserVM>();
            CreateMap<UserVM, User>();
            CreateMap<User, UserUpdateVM>();
            CreateMap<UserUpdateVM, User>();

        }
    }
}
