using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcPracownik.Data;
using MvcPracownik.Models;

namespace lab10.Controllers
{
    public class Student_ZajeciaController : Controller
    {
        private readonly MvcPracownikContext _context;

        public Student_ZajeciaController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: Student_Zajecia
        public async Task<IActionResult> Index()
        {
              return _context.Student_Zajecia != null ? 
                          View(await _context.Student_Zajecia.ToListAsync()) :
                          Problem("Entity set 'MvcPracownikContext.Student_Zajecia'  is null.");
        }

        // GET: Student_Zajecia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Student_Zajecia == null)
            {
                return NotFound();
            }

            var student_Zajecia = await _context.Student_Zajecia
                .FirstOrDefaultAsync(m => m.Student_Zajecia_ID == id);
            if (student_Zajecia == null)
            {
                return NotFound();
            }

            return View(student_Zajecia);
        }

        // GET: Student_Zajecia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student_Zajecia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Student_Zajecia_ID")] Student_Zajecia student_Zajecia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student_Zajecia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student_Zajecia);
        }

        // GET: Student_Zajecia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Student_Zajecia == null)
            {
                return NotFound();
            }

            var student_Zajecia = await _context.Student_Zajecia.FindAsync(id);
            if (student_Zajecia == null)
            {
                return NotFound();
            }
            return View(student_Zajecia);
        }

        // POST: Student_Zajecia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Student_Zajecia_ID")] Student_Zajecia student_Zajecia)
        {
            if (id != student_Zajecia.Student_Zajecia_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student_Zajecia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Student_ZajeciaExists(student_Zajecia.Student_Zajecia_ID))
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
            return View(student_Zajecia);
        }

        // GET: Student_Zajecia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Student_Zajecia == null)
            {
                return NotFound();
            }

            var student_Zajecia = await _context.Student_Zajecia
                .FirstOrDefaultAsync(m => m.Student_Zajecia_ID == id);
            if (student_Zajecia == null)
            {
                return NotFound();
            }

            return View(student_Zajecia);
        }

        // POST: Student_Zajecia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Student_Zajecia == null)
            {
                return Problem("Entity set 'MvcPracownikContext.Student_Zajecia'  is null.");
            }
            var student_Zajecia = await _context.Student_Zajecia.FindAsync(id);
            if (student_Zajecia != null)
            {
                _context.Student_Zajecia.Remove(student_Zajecia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Student_ZajeciaExists(int id)
        {
          return (_context.Student_Zajecia?.Any(e => e.Student_Zajecia_ID == id)).GetValueOrDefault();
        }
    }
}
