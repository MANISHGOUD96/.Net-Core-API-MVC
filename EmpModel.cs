using System.ComponentModel.DataAnnotations;

namespace Mk_Core_API_MVC.Models
{
    public class EmpModel
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Age { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? ZipCode { get; set; }
        [Required]
        public string? Company { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Department { get; set; }

        [Required]
        public int Selary { get; set; }
    }
}
