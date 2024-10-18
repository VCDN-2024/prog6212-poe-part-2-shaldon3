using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProgTest.Models;

namespace ProgTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // DbSet for each model (table)
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<ProgrammeCoordinator> ProgrammeCoordinators { get; set; }
        public DbSet<AcademicManager> AcademicManagers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ClaimApproval> ClaimApprovals { get; set; }

        // Additional configuration can be done here, such as setting up relationships or overriding default conventions
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensures the identity-related tables are created

            // Configuring relationships for foreign keys, e.g. Claim-Document, Claim-ClaimApproval, etc.
            modelBuilder.Entity<Claim>()
                .HasMany(c => c.Documents)
                .WithOne(d => d.Claim)
                .HasForeignKey(d => d.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimApproval>()
                .HasOne(ca => ca.Claim)
                .WithMany()
                .HasForeignKey(ca => ca.ClaimId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClaimApproval>()
                .HasOne(ca => ca.Coordinator)
                .WithMany()
                .HasForeignKey(ca => ca.CoordinatorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ClaimApproval>()
                .HasOne(ca => ca.Manager)
                .WithMany()
                .HasForeignKey(ca => ca.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            // You can add additional model relationships/configurations as needed
        }
    }
}