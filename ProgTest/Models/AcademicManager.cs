using System.ComponentModel.DataAnnotations;

namespace ProgTest.Models
{
    public class AcademicManager
    {
        [Key]
        public int ManagerId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } // Full name of the Academic Manager

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Email address of the Manager

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } // Phone number of the Manager
    }
}
