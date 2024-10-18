using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgTest.Data;
using ProgTest.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProgTest.Controllers
{
    public class ClaimTrackerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimTrackerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClaimTracker
        public async Task<IActionResult> Index()
        {
            var claimApprovals = (from c in _context.Claims
                                  join ca in _context.ClaimApprovals
                                  on c.ClaimId equals ca.ClaimId into caGroup
                                  from ca in caGroup.DefaultIfEmpty() // Left join
                                  select new ClaimApprovalViewModel
                                  {
                                      ClaimId = c.ClaimId,
                                      LecturerName = c.LecturerName,
                                      HoursWorked = c.HoursWorked,
                                      HourlyRate = c.HourlyRate,
                                      TotalAmount = c.TotalAmount,
                                      ClaimDate = c.ClaimDate,
                                      IsSubmitted = c.IsSubmitted,
                                      ApproverRole = ca != null ? ca.ApproverRole : "Pending", // Default to "Pending"
                                      IsApproved = ca != null && ca.IsApproved // Default to false if no approval found
                                  }).AsEnumerable() // Switch to client-side evaluation here
                         .GroupBy(claim => new
                         {
                             claim.ClaimId,
                             claim.LecturerName,
                             claim.HoursWorked,
                             claim.HourlyRate,
                             claim.TotalAmount,
                             claim.ClaimDate,
                             claim.IsSubmitted
                         })
                         .Select(group => new ClaimApprovalViewModel
                         {
                             ClaimId = group.Key.ClaimId,
                             LecturerName = group.Key.LecturerName,
                             HoursWorked = group.Key.HoursWorked,
                             HourlyRate = group.Key.HourlyRate,
                             TotalAmount = group.Key.TotalAmount,
                             ClaimDate = group.Key.ClaimDate,
                             IsSubmitted = group.Key.IsSubmitted,
                             ApproverRole = string.Join(", ", group.Select(g => g.ApproverRole)),
                             IsApproved = group.Any(g => g.IsApproved)
                         }).ToList(); // Use ToList() for in-memory collections

            return View(claimApprovals);
        }
    }
}
