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
    public class BudynekController : Controller
    {
        private readonly MvcPracownikContext _context;

        public BudynekController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: Budynek
        public async Task<IActionResult> Index()
        {
              return _context.Budynek != null ? 
                          View(await _context.Budynek.ToListAsync()) :
                          Problem("Entity set 'MvcPracownikContext.Budynek'  is null.");
        }

        // GET: Budynek/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Budynek == null)
            {
                return NotFound();
            }

            var budynek = await _context.Budynek
                .FirstOrDefaultAsync(m => m.Id_budynku == id);
            if (budynek == null)
            {
                return NotFound();
            }

            return View(budynek);
        }

        // GET: Budynek/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Budynek/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_budynku,Nazwa")] Budynek budynek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(budynek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(budynek);
        }

        // GET: Budynek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Budynek == null)
            {
                return NotFound();
            }

            var budynek = await _context.Budynek.FindAsync(id);
            if (budynek == null)
            {
                return NotFound();
            }
            return View(budynek);
        }

        // POST: Budynek/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_budynku,Nazwa")] Budynek budynek)
        {
            if (id != budynek.Id_budynku)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(budynek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BudynekExists(budynek.Id_budynku))
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
            return View(budynek);
        }

        // GET: Budynek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Budynek == null)
            {
                return NotFound();
            }

            var budynek = await _context.Budynek
                .FirstOrDefaultAsync(m => m.Id_budynku == id);
            if (budynek == null)
            {
                return NotFound();
            }

            return View(budynek);
        }

        // POST: Budynek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Budynek == null)
            {
                return Problem("Entity set 'MvcPracownikContext.Budynek'  is null.");
            }
            var budynek = await _context.Budynek.FindAsync(id);
            if (budynek != null)
            {
                _context.Budynek.Remove(budynek);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudynekExists(int id)
        {
          return (_context.Budynek?.Any(e => e.Id_budynku == id)).GetValueOrDefault();
        }
    }
}
