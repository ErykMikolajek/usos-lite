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
            if (HttpContext.Session.GetString("Logged in") == null)
                return View("Views/Auth/Login.cshtml");
            var zaj = _context.Zajecia.Include(p => p.Budynek).AsNoTracking();
            return View(await zaj.ToListAsync());
        }

        // GET: Zajecia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zajecia == null)
            {
                return NotFound();
            }

            var zajecia = await _context.Zajecia
                .Include(p => p.Budynek)
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
            PopulateBudynekDropDownList();
            return View();
        }

        // POST: Zajecia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id_zajec,Nazwa")] Zajecia zajecia,
            IFormCollection form)
        {
            string budynekValue = form["BudynekDropDown"];
            if (ModelState.IsValid)
            {

                Budynek budynek = null;
                if (budynekValue != "-1")
                {
                    var ee = _context.Budynek.Where(e => e.Id_budynku == int.Parse(budynekValue));
                    if (ee.Count() > 0)
                        budynek = ee.First();
                }
                zajecia.Budynek = budynek;

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

            var zajecia = _context.Zajecia.Where(p => p.Id_zajec == id)
                .Include(p => p.Budynek).First();
            if (zajecia == null)
            {
                return NotFound();
            }
            if (zajecia.Budynek != null)
            {
                PopulateBudynekDropDownList(zajecia.Budynek.Id_budynku);
            }
            else
            {
                PopulateBudynekDropDownList();
            }

            return View(zajecia);
        }

        // POST: Zajecia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_zajec,Nazwa")] Zajecia zajecia, IFormCollection form)
        {
            if (id != zajecia.Id_zajec)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string budynekValue = form["BudynekDropDown"];


                    Budynek budynek = null;
                    if (budynekValue != "-1")
                    {
                        var ee = _context.Budynek.Where(e => e.Id_budynku == int.Parse(budynekValue));
                        if (ee.Count() > 0)
                            budynek = ee.First();
                    }
                    zajecia.Budynek = budynek;

                    //Aby kontekst śledził zmiany w referowanych kolumnach etat oraz zespol
                    //należy "dostać" się do obiektu przez dbContext i dołączyć obiekty etat
                    //i zespol. Bez tego kolumny etat i zespół nie będą mogły być zmodyfikowane
                    //wartością NULL-ową, czyli nie będzie się dało usunąć powiązania. 
                    //Ustawienie na inną wartość niż NULL będzie działać przy "zwykłym"
                    // _context.Update(pracownik);
                    Zajecia pp = _context.Zajecia.Where(p => p.Id_zajec == id)
                    .Include(p => p.Budynek)
                    .First();
                    pp.Budynek = budynek;
                    pp.Nazwa = zajecia.Nazwa;

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

            // var zajecia = await _context.Zajecia
            //     .FirstOrDefaultAsync(m => m.Id_zajec == id);
            var zajecia = _context.Zajecia.Where(p => p.Id_zajec == id)
                .Include(p => p.Budynek).First();
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

        private void PopulateBudynekDropDownList(object selectedBudynek = null)
        {
            var wybranyBudynek = from z in _context.Budynek
                                 orderby z.Nazwa
                                 select z;
            var res = wybranyBudynek.AsNoTracking();
            ViewBag.BudynekId = new SelectList(res, "Id_budynku", "Nazwa", selectedBudynek);
        }


    }
}
