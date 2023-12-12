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
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int author_id { get; set; }
        [Required]
        [StringLength(50)]
        public string last_name { get; set; }
        [Required]
        [StringLength(50)]
        public string first_name { get; set; }
        [Required]
        [StringLength(50)]
        public string email_address { get; set; }
        [StringLength(30)]
        public string? phone { get; set; }
        [StringLength(50)]
        public string? address { get; set; }
        [StringLength(50)]
        public string? city { get; set; }
        [StringLength(50)]
        public string? state { get; set; }
        [StringLength(50)]
        public string? zip { get; set; }

        public ICollection<BookAuthor>? BookAuthor { get; set; }
    }
}
