using System.ComponentModel.DataAnnotations;

namespace Assigment02_WebAPI.ViewModel
{
    public class PublisherVM
    {
        //public int pub_id { get; set; }
        //[Required]
        //[MaxLength(30)]
        public string publisher_name { get; set; }
        [MaxLength(50)]
        public string? city { get; set; }
        [MaxLength(50)]
        public string? state { get; set; }
        [MaxLength(50)]
        public string? country { get; set; }
    }

    public class PublisherUpdateVM
    {
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
    }
}
