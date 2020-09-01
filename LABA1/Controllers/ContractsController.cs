using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LABA1;
using Microsoft.AspNetCore.Authorization;
using LABA1.Models;

namespace LABA1.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class ContractsController : Controller
    {
        private readonly MyDBContext _context;

        public ContractsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(SortContracts sortOrder = SortContracts.RenterAsc)
        {
            //var myDBContext = _context.Contracts.Include(c => c.Car).Include(c => c.Renter);
            IQueryable<Contracts> myDBContext = _context.Contracts.Include(c => c.Car).Include(c => c.Renter);

            ViewData["RenterSort"] = sortOrder == SortContracts.RenterAsc ? SortContracts.RenterDesc: SortContracts.RenterAsc;
            ViewData["CarSort"] = sortOrder == SortContracts.CarAsc? SortContracts.CarDesc: SortContracts.CarAsc;
            ViewData["StartDateSort"] = sortOrder == SortContracts.StartDateAsc ? SortContracts.StartDateDesc : SortContracts.StartDateAsc;
            ViewData["DayNumberSort"] = sortOrder == SortContracts.DayNumberAsc ? SortContracts.DayNumberDesc: SortContracts.DayNumberAsc;
            ViewData["DayPriceSort"] = sortOrder == SortContracts.DayPriceAsc ? SortContracts.DayPriceDesc : SortContracts.DayPriceAsc;
            myDBContext = sortOrder switch
            {
                SortContracts.RenterAsc => myDBContext.OrderBy(s => s.Renter),
                SortContracts.RenterDesc => myDBContext.OrderByDescending(s => s.Renter),
                SortContracts.CarAsc => myDBContext.OrderBy(s => s.Car),
                SortContracts.CarDesc => myDBContext.OrderByDescending(s => s.Car),
                SortContracts.StartDateAsc => myDBContext.OrderBy(s => s.StartDate),
                SortContracts.StartDateDesc => myDBContext.OrderByDescending(s => s.StartDate),
                SortContracts.DayNumberAsc => myDBContext.OrderBy(s => s.DayNumber),
                SortContracts.DayNumberDesc => myDBContext.OrderByDescending(s => s.DayNumber),
                SortContracts.DayPriceAsc => myDBContext.OrderBy(s => s.DayPrice),
                SortContracts.DayPriceDesc => myDBContext.OrderByDescending(s => s.DayPrice),
                _ => myDBContext.OrderBy(s => s.Renter),
            };
            return View(await myDBContext.AsNoTracking().ToListAsync());
            //return View(await myDBContext.ToListAsync());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _context.Contracts
                .Include(c => c.Car)
                .Include(c => c.Renter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contracts == null)
            {
                return NotFound();
            }

            return View(contracts);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "GovNumber");
            ViewData["RenterId"] = new SelectList(_context.Renters, "Id", "Phone");
            return View();
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RenterId,CarId,StartDate,DayNumber,DayPrice")] Contracts contracts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contracts);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "GovNumber", contracts.CarId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "Id", "Phone", contracts.RenterId);
            return View(contracts);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _context.Contracts.FindAsync(id);
            if (contracts == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "GovNumber", contracts.CarId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "Id", "Phone", contracts.RenterId);
            return View(contracts);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RenterId,CarId,StartDate,DayNumber,DayPrice")] Contracts contracts)
        {
            if (id != contracts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contracts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContractsExists(contracts.Id))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "GovNumber", contracts.CarId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "Id", "Phone", contracts.RenterId);
            return View(contracts);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contracts = await _context.Contracts
                .Include(c => c.Car)
                .Include(c => c.Renter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contracts == null)
            {
                return NotFound();
            }

            return View(contracts);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contracts = await _context.Contracts.FindAsync(id);
            _context.Contracts.Remove(contracts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContractsExists(int id)
        {
            return _context.Contracts.Any(e => e.Id == id);
        }
    }
}
