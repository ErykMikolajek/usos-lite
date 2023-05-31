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
    public class ZajeciaController : Controller
    {
        private readonly MvcPracownikContext _context;

        public ZajeciaController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: Zajecia
        public async Task<IActionResult> Index()
        {
              return _context.Zajecia != null ? 
                          View(await _context.Zajecia.ToListAsync()) :
                          Problem("Entity set 'MvcPracownikContext.Zajecia'  is null.");
        }

        // GET: Zajecia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zajecia == null)
            {
                return NotFound();
            }

            var zajecia = await _context.Zajecia
                .FirstOrDefaultAsync(m => m.Id_zajec == id);
            if (zajecia == null)
            {
                return NotFound();
            }

            return View(zajecia);
        }

        // GET: Zajecia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zajecia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_zajec,Nazwa")] Zajecia zajecia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zajecia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zajecia);
        }

        // GET: Zajecia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Zajecia == null)
            {
                return NotFound();
            }

            var zajecia = await _context.Zajecia.FindAsync(id);
            if (zajecia == null)
            {
                return NotFound();
            }
            return View(zajecia);
        }

        // POST: Zajecia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_zajec,Nazwa")] Zajecia zajecia)
        {
            if (id != zajecia.Id_zajec)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zajecia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZajeciaExists(zajecia.Id_zajec))
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
            return View(zajecia);
        }

        // GET: Zajecia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Zajecia == null)
            {
                return NotFound();
            }

            var zajecia = await _context.Zajecia
                .FirstOrDefaultAsync(m => m.Id_zajec == id);
            if (zajecia == null)
            {
                return NotFound();
            }

            return View(zajecia);
        }

        // POST: Zajecia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Zajecia == null)
            {
                return Problem("Entity set 'MvcPracownikContext.Zajecia'  is null.");
            }
            var zajecia = await _context.Zajecia.FindAsync(id);
            if (zajecia != null)
            {
                _context.Zajecia.Remove(zajecia);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZajeciaExists(int id)
        {
          return (_context.Zajecia?.Any(e => e.Id_zajec == id)).GetValueOrDefault();
        }
    }
}
