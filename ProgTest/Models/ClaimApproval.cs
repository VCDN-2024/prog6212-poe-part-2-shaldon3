using System.ComponentModel.DataAnnotations;

namespace ProgTest.Models
{
    public class ClaimApproval
    {
        [Key]
        public int ApprovalId { get; set; } // Primary Key

        [Required]
        public int ClaimId { get; set; } // Foreign Key linking to the Claim

        [Required]
        public DateTime ApprovalDate { get; set; } // Date when the claim was approved

        [Required]
        public bool IsApproved { get; set; } // Status of approval (true for approved, false for rejected)

        [Required]
        public string ApproverRole { get; set; } // Role of the approver (Coordinator/Manager)

        // Foreign key to Programme Coordinator (if applicable)
        public int? CoordinatorId { get; set; }
        public ProgrammeCoordinator Coordinator { get; set; } // Navigation property to Programme Coordinator

        // Foreign key to Academic Manager (if applicable)
        public int? ManagerId { get; set; }
        public AcademicManager Manager { get; set; } // Navigation property to Academic Manager

        // Navigation property to Claim
        public Claim Claim { get; set; } // Navigation property to the associated claim

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ApproverRole == "Coordinator" && !CoordinatorId.HasValue)
            {
                yield return new ValidationResult("CoordinatorId is required when the approver role is Coordinator.", new[] { "CoordinatorId" });
            }
            else if (ApproverRole == "Manager" && !ManagerId.HasValue)
            {
                yield return new ValidationResult("ManagerId is required when the approver role is Manager.", new[] { "ManagerId" });
            }
        }
    }
}
