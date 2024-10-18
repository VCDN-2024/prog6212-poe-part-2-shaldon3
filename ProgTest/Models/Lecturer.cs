using System.ComponentModel.DataAnnotations;

namespace ProgTest.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } // Full name of the Lecturer

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Email address of the Lecturer

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } // Phone number of the Lecturer
    }
}
