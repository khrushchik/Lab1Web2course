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
    [Authorize(Roles ="admin, user")]
    public class CarsController : Controller
    {
        private readonly MyDBContext _context;

        public CarsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(SortCars sortOrder=SortCars.YearAsc)
        {
            //new
            IQueryable<Cars> myDBContext=_context.Cars.Include(c => c.Body).Include(c => c.Color).Include(c => c.Label).Include(c => c.Landlord).Include(c => c.Transmission);
            //new

            //var myDBContext = _context.Cars.Include(c => c.Body).Include(c => c.Color).Include(c => c.Label).Include(c => c.Landlord).Include(c => c.Transmission);

            //new
            ViewData["GovNumberSort"] = sortOrder == SortCars.GovNumberAsc ? SortCars.GovNumberDesc : SortCars.GovNumberAsc;
            ViewData["PriceSort"] = sortOrder == SortCars.PriceAsc ? SortCars.PriceDesc : SortCars.PriceAsc;
            ViewData["YearSort"] = sortOrder == SortCars.YearAsc ? SortCars.YearDesc : SortCars.YearAsc;
            ViewData["BodySort"] = sortOrder == SortCars.BodyAsc ? SortCars.BodyDesc : SortCars.BodyAsc;
            ViewData["ColorSort"] = sortOrder == SortCars.ColorAsc ? SortCars.ColorDesc : SortCars.ColorAsc;
            ViewData["LabelSort"] = sortOrder == SortCars.LabelAsc ? SortCars.LabelDesc : SortCars.LabelAsc;
            ViewData["LandlordSort"] = sortOrder == SortCars.LandlordAsc ? SortCars.LandlordDesc : SortCars.LandlordAsc;
            ViewData["TransmissionSort"] = sortOrder == SortCars.TransmissionAsc ? SortCars.TransmissionDesc : SortCars.TransmissionAsc;
            myDBContext = sortOrder switch
            {
                SortCars.GovNumberAsc=>myDBContext.OrderBy(s=>s.GovNumber),
                SortCars.GovNumberDesc=>myDBContext.OrderByDescending(s=>s.GovNumber),
                SortCars.PriceAsc => myDBContext.OrderBy(s => s.Price),
                SortCars.PriceDesc => myDBContext.OrderByDescending(s => s.Price),
                SortCars.YearAsc => myDBContext.OrderBy(s => s.Year),
                SortCars.YearDesc=>myDBContext.OrderByDescending(s=>s.Year),
                SortCars.BodyAsc => myDBContext.OrderBy(s => s.Body),
                SortCars.BodyDesc=> myDBContext.OrderByDescending(s=>s.Body),
                SortCars.ColorAsc => myDBContext.OrderBy(s => s.Color),
                SortCars.ColorDesc=> myDBContext.OrderByDescending(s=>s.Color),
                SortCars.LabelAsc => myDBContext.OrderBy(s => s.Label),
                SortCars.LabelDesc=> myDBContext.OrderByDescending(s=>s.Label),
                SortCars.LandlordAsc => myDBContext.OrderBy(s => s.Landlord),
                SortCars.LandlordDesc=> myDBContext.OrderByDescending(s=>s.Landlord),
                SortCars.TransmissionAsc => myDBContext.OrderBy(s => s.Transmission),
                SortCars.TransmissionDesc=> myDBContext.OrderByDescending(s=>s.Transmission),
                _=>myDBContext.OrderBy(s=>s.Year),
            };
            //new

            //return View(await myDBContext.ToListAsync());

            //new
            return View(await myDBContext.AsNoTracking().ToListAsync());
            //new
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars
                .Include(c => c.Body)
                .Include(c => c.Color)
                .Include(c => c.Label)
                .Include(c => c.Landlord)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "Body");
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Color");
            ViewData["LabelId"] = new SelectList(_context.Labels, "LableId", "Lable");
            ViewData["LandlordId"] = new SelectList(_context.Landlords, "Id", "Name");
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "TransmissionId", "Trasmission");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LabelId,BodyId,ColorId,Year,TransmissionId,Price,LandlordId,GovNumber")] Cars cars)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cars);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "Body", cars.BodyId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Color", cars.ColorId);
            ViewData["LabelId"] = new SelectList(_context.Labels, "LableId", "Lable", cars.LabelId);
            ViewData["LandlordId"] = new SelectList(_context.Landlords, "Id", "Name", cars.LandlordId);
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "TransmissionId", "Trasmission", cars.TransmissionId);
            return View(cars);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars.FindAsync(id);
            if (cars == null)
            {
                return NotFound();
            }
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "Body", cars.BodyId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Color", cars.ColorId);
            ViewData["LabelId"] = new SelectList(_context.Labels, "LableId", "Lable", cars.LabelId);
            ViewData["LandlordId"] = new SelectList(_context.Landlords, "Id", "Name", cars.LandlordId);
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "TransmissionId", "Trasmission", cars.TransmissionId);
            return View(cars);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LabelId,BodyId,ColorId,Year,TransmissionId,Price,LandlordId,GovNumber")] Cars cars)
        {
            if (id != cars.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cars);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarsExists(cars.Id))
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
            ViewData["BodyId"] = new SelectList(_context.Bodies, "BodyId", "Body", cars.BodyId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "ColorId", "Color", cars.ColorId);
            ViewData["LabelId"] = new SelectList(_context.Labels, "LableId", "Lable", cars.LabelId);
            ViewData["LandlordId"] = new SelectList(_context.Landlords, "Id", "Name", cars.LandlordId);
            ViewData["TransmissionId"] = new SelectList(_context.Transmissions, "TransmissionId", "Trasmission", cars.TransmissionId);
            return View(cars);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cars = await _context.Cars
                .Include(c => c.Body)
                .Include(c => c.Color)
                .Include(c => c.Label)
                .Include(c => c.Landlord)
                .Include(c => c.Transmission)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cars == null)
            {
                return NotFound();
            }

            return View(cars);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //new
            IQueryable<Contracts> contracts = from o in _context.Contracts where o.CarId == id select o;
            Cars cars = _context.Cars.Find(id);
            foreach(Contracts contracts1 in contracts)
            {
                _context.Contracts.Remove(contracts1);
            }
            _context.Cars.Remove(cars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //new

            /*var cars = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(cars);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

        private bool CarsExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
