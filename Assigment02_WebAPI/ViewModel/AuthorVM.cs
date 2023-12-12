using System.ComponentModel.DataAnnotations;

namespace Assigment02_WebAPI.ViewModel
{
    public class AuthorVM
    {
        //public int author_id { get; set; }
        //[Required]
        //[StringLength(50)]
        public string last_name { get; set; }
        [Required]
        [StringLength(50)]
        public string first_name { get; set; }
        [Required]
        [StringLength(50)]
        public string email_address { get; set; }
        [StringLength(50)]
        public string? phone { get; set; }
        [StringLength(50)]
        public string? address { get; set; }
        [StringLength(50)]
        public string? city { get; set; }
        [StringLength(50)]
        public string? state { get; set; }
        [StringLength(50)]
        public string? zip { get; set; }
    }
    public class AuthorUpdateVM
    {
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
        [StringLength(50)]
        public string? phone { get; set; }
        [StringLength(50)]
        public string? address { get; set; }
        [StringLength(50)]
        public string? city { get; set; }
        [StringLength(50)]
        public string? state { get; set; }
        [StringLength(50)]
        public string? zip { get; set; }
    }
   
}
