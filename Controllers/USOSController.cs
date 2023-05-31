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
    public class USOSController : Controller
    {
        private readonly MvcPracownikContext _context;

        public USOSController(MvcPracownikContext context)
        {
            _context = context;
        }

        // GET: USOS
        public async Task<IActionResult> Index()
        {
            var prac = _context.Pracownik.Include(p => p.Zajecia).AsNoTracking();
            return View(await prac.ToListAsync());
        }

        // GET: USOS/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik = await _context.Pracownik
                .Include(p => p.Zajecia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pracownik == null)
            {
                return NotFound();
            }

            return View(pracownik);
        }

        // GET: USOS/Create
        public IActionResult Create()
        {
            PopulateBudynekDropDownList();
            PopulateZajeciaDropDownList();
            return View();
        }

        // POST: USOS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,DataZatrudnienia")] Pracownik pracownik,
            IFormCollection form)
        {
            string zajeciaValue = form["ZajeciaDropDown"];
            if (ModelState.IsValid)
            {
                Zajecia zajecia = null;
                if (zajeciaValue != "-1")
                {
                    var ee = _context.Zajecia.Where(e => e.Id_zajec == int.Parse(zajeciaValue));
                    if (ee.Count() > 0)
                        zajecia = ee.First();
                }
                pracownik.Zajecia = zajecia;

                _context.Add(pracownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pracownik);
        }

        // GET: USOS/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pracownik == null)
            {
                return NotFound();
            }


            var pracownik = _context.Pracownik.Where(p => p.Id == id)
                .Include(p => p.Zajecia).First();
            if (pracownik == null)
            {
                return NotFound();
            }
            if (pracownik.Zajecia != null)
            {
                PopulateZajeciaDropDownList(pracownik.Zajecia.Id_zajec);
            }
            else
            {
                PopulateZajeciaDropDownList();
            }

            return View(pracownik);
        }

        // POST: USOS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Nazwisko,DataZatrudnienia")] Pracownik pracownik, IFormCollection form)
        {
            if (id != pracownik.Id)
                {
                    return NotFound();
                }


                if (ModelState.IsValid)
                {
                    try
                    {

                        //String etatValue = form["Etat.Id"];
                        //String zespolValue = form["Zespol.Id_zespolu"];
                        string zajeciaValue = form["ZajeciaDropDown"];


                        Zajecia zajecia = null;
                        if (zajeciaValue != "-1")
                        {
                            var ee = _context.Zajecia.Where(e => e.Id_zajec == int.Parse(zajeciaValue));
                            if (ee.Count() > 0)
                                zajecia = ee.First();
                        }
                        pracownik.Zajecia = zajecia;

                        //Aby kontekst śledził zmiany w referowanych kolumnach etat oraz zespol
                        //należy "dostać" się do obiektu przez dbContext i dołączyć obiekty etat
                        //i zespol. Bez tego kolumny etat i zespół nie będą mogły być zmodyfikowane
                        //wartością NULL-ową, czyli nie będzie się dało usunąć powiązania. 
                        //Ustawienie na inną wartość niż NULL będzie działać przy "zwykłym"
                        // _context.Update(pracownik);
                        Pracownik pp = _context.Pracownik.Where(p => p.Id == id)
                        .Include(p => p.Zajecia)
                        .First();
                        pp.Zajecia = zajecia;
                        pp.Imie = pracownik.Imie;
                        pp.Nazwisko = pracownik.Nazwisko;
                        pp.DataZatrudnienia = pracownik.DataZatrudnienia;

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PracownikExists(pracownik.Id))
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
            return View(pracownik);
        }

        // GET: USOS/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pracownik = _context.Pracownik.Where(p => p.Id == id)
                .Include(p => p.Zajecia).First();
            //var pracownik = await _context.Pracownicy
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (pracownik == null)
            {
                return NotFound();
            }

            return View(pracownik);
        }

        // POST: USOS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pracownik == null)
            {
                return Problem("Entity set 'MvcPracownikContext.Pracownik'  is null.");
            }
            var pracownik = await _context.Pracownik.FindAsync(id);
            if (pracownik != null)
            {
                _context.Pracownik.Remove(pracownik);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PracownikExists(int id)
        {
          return (_context.Pracownik?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void PopulateBudynekDropDownList(object selectedBudynek = null)
        {
            var wybraneBudynki = from b in _context.Budynek
                                orderby b.Nazwa
                                select b;
            var res = wybraneBudynki.AsNoTracking();
            ViewBag.BudynekID = new SelectList(res, "Id", "Nazwa", selectedBudynek);
        }

        private void PopulateZajeciaDropDownList(object selectedZajecia = null) 
        {
            var wybraneZajecia = from z in _context.Zajecia
                                orderby z.Nazwa
                                select z;
            var res = wybraneZajecia.AsNoTracking();
            ViewBag.ZajeciaID = new SelectList(res, "Id_zajec", "Nazwa", selectedZajecia);
        }
    }
}

/* TODO:

jest TYLKO create dla pracownika

brak pozostalych create edit w innych controlerach etc...

polaczenia miedzy tabelami sprawdzic

dropdown dla wielu elementow???

*/