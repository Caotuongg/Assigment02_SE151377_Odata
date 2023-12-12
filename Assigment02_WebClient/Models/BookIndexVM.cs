using Assigment02_BussinessObject;

namespace Assigment02_WebClient.Models
{
    public class BookIndexVM
    {
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public int ItemPerPage { get; set; }
        public int TotalValues { get; set; }
        public string Search { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}
