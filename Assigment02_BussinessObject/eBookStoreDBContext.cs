using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assigment02_BussinessObject
{
    public class eBookStoreDBContext : DbContext
    { 
        public eBookStoreDBContext() { }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<Publisher> Publisher { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookAuthor>()
                .HasKey(p => new { p.book_id, p.author_id });
            modelBuilder.Entity<BookAuthor>()
                .HasOne(p => p.Author)
                .WithMany(p => p.BookAuthor)
                .HasForeignKey(p => p.author_id);
            modelBuilder.Entity<BookAuthor>()
                .HasOne(p => p.Book)
                .WithMany(p => p.BookAuthor)
                .HasForeignKey(p => p.book_id);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            optionsBuilder.UseSqlServer(config["ConnectionStrings:eBookStore"]);
            optionsBuilder.UseSqlServer("server=(local);uid=sa;pwd=1234567890;database=eBookStoreDB;TrustServerCertificate=True");
        }


    }
}
