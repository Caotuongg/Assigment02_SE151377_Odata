using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assigment02_BussinessObject
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { get; set; }
        [Required]
        [MaxLength(200)]
        public string bookname { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public int type { get; set; }

        [Required]
        public int pub_id { get; set; }

        public virtual Publisher? Publisher { get; set; }

        [Required]
        public double? price { get; set; }
        public double? advance { get; set; }
        public int? royalty { get; set; }
        public double? ytd_sales { get; set; }
        [MaxLength(500)]
        public string? notes { get; set; }
        public DateTime? published_date { get; set; }

        public ICollection<BookAuthor>? BookAuthor { get; set; }
    }
}
