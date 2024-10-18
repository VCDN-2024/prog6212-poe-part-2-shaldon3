using System.ComponentModel.DataAnnotations;

namespace ProgTest.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; } // Primary Key

        [Required]
        [StringLength(100)]
        public string LecturerName { get; set; } // Full name of the Lecturer

        [Required]
        [Range(1, 500)]
        public double HoursWorked { get; set; } // Total hours worked by the Lecturer

        [Required]
        [Range(100, 10000)]
        public double HourlyRate { get; set; } // Lecturer's hourly rate

        [Required]
        [Range(0, double.MaxValue)]
        public double TotalAmount
        {
            get { return HoursWorked * HourlyRate; } // Automatically calculate the total claim amount
        }

        [Required]
        public DateTime ClaimDate { get; set; } // Date the claim was submitted

        [Required]
        public bool IsSubmitted { get; set; } // Whether the claim has been submitted by the Lecturer

        // Navigation property for the associated documents
        public ICollection<Document> Documents { get; set; } = new List<Document>(); // A claim can have multiple documents
    }
}
