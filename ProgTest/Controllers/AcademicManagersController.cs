using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProgTest.Data;
using ProgTest.Models;

namespace ProgTest.Controllers
{
    public class AcademicManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicManagersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicManagers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AcademicManagers.ToListAsync());
        }

        // GET: AcademicManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicManager = await _context.AcademicManagers
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (academicManager == null)
            {
                return NotFound();
            }

            return View(academicManager);
        }

        // GET: AcademicManagers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademicManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManagerId,FullName,Email,PhoneNumber")] AcademicManager academicManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academicManager);
        }

        // GET: AcademicManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicManager = await _context.AcademicManagers.FindAsync(id);
            if (academicManager == null)
            {
                return NotFound();
            }
            return View(academicManager);
        }

        // POST: AcademicManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManagerId,FullName,Email,PhoneNumber")] AcademicManager academicManager)
        {
            if (id != academicManager.ManagerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicManagerExists(academicManager.ManagerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(academicManager);
        }

        // GET: AcademicManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicManager = await _context.AcademicManagers
                .FirstOrDefaultAsync(m => m.ManagerId == id);
            if (academicManager == null)
            {
                return NotFound();
            }

            return View(academicManager);
        }

        // POST: AcademicManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicManager = await _context.AcademicManagers.FindAsync(id);
            if (academicManager != null)
            {
                _context.AcademicManagers.Remove(academicManager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicManagerExists(int id)
        {
            return _context.AcademicManagers.Any(e => e.ManagerId == id);
        }
    }
}
