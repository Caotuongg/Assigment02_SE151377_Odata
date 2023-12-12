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
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pub_id { get; set; }
        [Required]
        [MaxLength(30)]
        public string publisher_name { get; set; }
        [MaxLength(50)]
        public string? city { get; set; }
        [MaxLength(50)]
        public string? state { get; set; }
        [MaxLength(50)]
        public string? country { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}
