using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProgTest.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; } // Primary Key

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } // Original file name

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } // Path where the file is stored

        [Required]
        [Range(0, long.MaxValue)]
        public long FileSize { get; set; } // File size in bytes

        [Required]
        public DateTime UploadedDate { get; set; } // Date when the document was uploaded

        [Required]
        public int ClaimId { get; set; } // Foreign key to associate document with a claim

        // Navigation Property
        [ForeignKey("ClaimId")]
        public Claim Claim { get; set; } // Navigation property to associate with Claim

        // Added property for file upload
        [NotMapped]
        public IFormFile File { get; set; } // This is for the uploaded file only, not saved to the database
    }
}
