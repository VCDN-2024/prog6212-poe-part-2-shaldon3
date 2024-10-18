namespace ProgTest.Models
{
    public class ClaimStatusViewModel
    {
        public int ClaimId { get; set; }
        public string LecturerName { get; set; }
        public double TotalAmount { get; set; }
        public string ApprovalStatus { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public DateTime ClaimDate { get; set; }
    }
}
