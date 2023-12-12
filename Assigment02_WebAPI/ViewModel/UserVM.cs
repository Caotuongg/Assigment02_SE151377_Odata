using Assigment02_BussinessObject;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment02_WebAPI.ViewModel
{
    public class UserVM
    {
        //public int user_id { get; set; }
        //[Required]
        //[StringLength(50)]
        public string email_address { get; set; }
        [Required]
        [StringLength(50)]
        public string password { get; set; }
        public string? source { get; set; }
        [Required, StringLength(50)]
        public string first_name { get; set; }
        [Required, StringLength(50)]
        public string middle_name { get; set; }
        [Required, StringLength(50)]
        public string last_name { get; set; }

        [ForeignKey("Role")]
        public int? role_id { get; set; }
        //public Role? Role { get; set; }

        [ForeignKey("Publisher")]
        public int? pub_id { get; set; }
        //public Publisher? Publisher { get; set; }

        public DateTime? hire_date { get; set; }
    }

    public class UserUpdateVM
    {
        public int user_id { get; set; }
        [Required]
        [StringLength(50)]
        public string email_address { get; set; }
        [Required]
        [StringLength(50)]
        public string password { get; set; }
        public string? source { get; set; }
        [Required, StringLength(50)]
        public string first_name { get; set; }
        [Required, StringLength(50)]
        public string middle_name { get; set; }
        [Required, StringLength(50)]
        public string last_name { get; set; }

        //[ForeignKey("Role")]
        //public int? role_id { get; set; }
        //public Role? Role { get; set; }

        //[ForeignKey("Publisher")]
        //public int? pub_id { get; set; }
        //public Publisher? Publisher { get; set; }

        public DateTime? hire_date { get; set; }
    }

    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
