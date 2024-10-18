using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgTest.Data;
using ProgTest.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProgTest.Controllers
{
    public class ClaimApprovalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClaimApprovalController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var claimApprovals = await (from c in _context.Claims
                                        join ca in _context.ClaimApprovals
                                        on c.ClaimId equals ca.ClaimId into caGroup
                                        select new ClaimApprovalViewModel
                                        {
                                            ClaimId = c.ClaimId,
                                            LecturerName = c.LecturerName,
                                            HoursWorked = c.HoursWorked,
                                            HourlyRate = c.HourlyRate,
                                            TotalAmount = c.TotalAmount,
                                            ClaimDate = c.ClaimDate,
                                            IsSubmitted = c.IsSubmitted,
                                            // Use aggregate functions to summarize approval statuses
                                            ApproverRole = caGroup.FirstOrDefault().ApproverRole, // If you need to show a role, adjust accordingly
                                            IsCoordinatorApproved = caGroup.Any(ca => ca.ApproverRole == "Coordinator" && ca.IsApproved),
                                            IsManagerApproved = caGroup.Any(ca => ca.ApproverRole == "Manager" && ca.IsApproved)
                                        }).ToListAsync();

            return View(claimApprovals);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);

            if (claim == null)
            {
                return NotFound(); // Handle case where claim itself is not found
            }

            var claimApprovals = await _context.ClaimApprovals
                .Where(ca => ca.ClaimId == id)
                .ToListAsync();

            // Initialize a new view model for claim approval
            var viewModel = new ClaimApprovalViewModel
            {
                ClaimId = claim.ClaimId,
                LecturerName = claim.LecturerName,
                HoursWorked = claim.HoursWorked,
                HourlyRate = claim.HourlyRate,
                TotalAmount = claim.TotalAmount,
                ClaimDate = claim.ClaimDate,
                // Assuming you're setting roles here; adjust if you want to manage roles differently
                IsApproved = claimApprovals.Any(ca => ca.IsApproved) // Set to true if any approval found
            };

            // Initialize specific approval status for each role
            viewModel.IsCoordinatorApproved = claimApprovals.Any(ca => ca.ApproverRole == "Coordinator" && ca.IsApproved);
            viewModel.IsManagerApproved = claimApprovals.Any(ca => ca.ApproverRole == "Manager" && ca.IsApproved);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClaimApprovalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var claimExists = await _context.Claims.AnyAsync(c => c.ClaimId == model.ClaimId);
                if (!claimExists)
                {
                    ModelState.AddModelError("", "The Claim ID does not exist.");
                    return View(model);
                }

                // Retrieve existing approvals
                var claimApprovals = await _context.ClaimApprovals
                    .Where(ca => ca.ClaimId == model.ClaimId)
                    .ToListAsync();

                // Process Coordinator approval
                var coordinatorApproval = claimApprovals.FirstOrDefault(ca => ca.ApproverRole == "Coordinator");
                if (coordinatorApproval == null)
                {
                    // Create new entry for Coordinator
                    coordinatorApproval = new ClaimApproval
                    {
                        ClaimId = model.ClaimId,
                        ApproverRole = "Coordinator",
                        IsApproved = model.IsCoordinatorApproved // Assuming you have this in your ViewModel
                    };
                    _context.ClaimApprovals.Add(coordinatorApproval);
                }
                else
                {
                    // Update existing Coordinator approval
                    coordinatorApproval.IsApproved = model.IsCoordinatorApproved;
                }

                // Process Manager approval
                var managerApproval = claimApprovals.FirstOrDefault(ca => ca.ApproverRole == "Manager");
                if (managerApproval == null)
                {
                    // Create new entry for Manager
                    managerApproval = new ClaimApproval
                    {
                        ClaimId = model.ClaimId,
                        ApproverRole = "Manager",
                        IsApproved = model.IsManagerApproved // Assuming you have this in your ViewModel
                    };
                    _context.ClaimApprovals.Add(managerApproval);
                }
                else
                {
                    // Update existing Manager approval
                    managerApproval.IsApproved = model.IsManagerApproved;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}
