using Assigment02_BussinessObject;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assigment02_WebAPI.ViewModel
{
    public class BookVM
    {
        //public int book_id { get; set; }
        //[Required]
        //[MaxLength(200)]
        public string bookname { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
       
        public int type { get; set; }
        [ForeignKey("Publisher")]
        public int? pub_id { get; set; }
        [Required]
        
        public double? price { get; set; }
        public double? advance { get; set; }
        public int? royalty { get; set; }
        public double? ytd_sales { get; set; }
        
        public string? notes { get; set; }
        
        public DateTime? published_date { get; set; }
    }

    public class BookUpdateVM
    {
        public int book_id { get; set; }
        [Required]
        [MaxLength(200)]
        public string title { get; set; }
        [Required]

        public int type { get; set; }
        [ForeignKey("Publisher")]
        public int? pub_id { get; set; }
        [Required]

        public double? price { get; set; }
        public double? advance { get; set; }
        public int? royalty { get; set; }
        public double? ytd_sales { get; set; }

        public string? notes { get; set; }

        public DateTime? published_date { get; set; }
    }

}
