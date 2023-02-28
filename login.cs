using System.ComponentModel.DataAnnotations;

namespace Mk_Core_API_MVC.Models
{
    public class login
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
