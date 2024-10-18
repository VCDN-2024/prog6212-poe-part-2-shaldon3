using System.ComponentModel.DataAnnotations;

namespace ProgTest.Models
{
    public class ClaimApprovalViewModel
    {
        public int ClaimId { get; set; } // Reference to Claim ID

        [Display(Name = "Lecturer Name")]
        public string LecturerName { get; set; }

        [Display(Name = "Hours Worked")]
        public double HoursWorked { get; set; }

        [Display(Name = "Hourly Rate")]
        public double HourlyRate { get; set; }

        [Display(Name = "Total Amount")]
        public double TotalAmount { get; set; }

        [Display(Name = "Claim Date")]
        public DateTime ClaimDate { get; set; }

        [Display(Name = "Is Submitted")]
        public bool IsSubmitted { get; set; }

        [Required]
        [Display(Name = "Approver Role")]
        public string ApproverRole { get; set; } // Role of the approver (Coordinator/Manager)

        // Approver details for both roles
        [Display(Name = "Coordinator Approval Status")]
        public bool IsCoordinatorApproved { get; set; } // Coordinator approval status

        [Display(Name = "Manager Approval Status")]
        public bool IsManagerApproved { get; set; } // Manager approval status

        [Required]
        public bool IsApproved { get; set; } // Status of approval (true for approved, false for rejected)

      
    }
}
