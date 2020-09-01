using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LABA1;

namespace LABA1.Controllers
{
    public class TransmissionsController : Controller
    {
        private readonly MyDBContext _context;

        public TransmissionsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Transmissions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transmissions.ToListAsync());
        }

        // GET: Transmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transmissions = await _context.Transmissions
                .FirstOrDefaultAsync(m => m.TransmissionId == id);
            if (transmissions == null)
            {
                return NotFound();
            }

            return View(transmissions);
        }

        // GET: Transmissions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transmissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransmissionId,Trasmission")] Transmissions transmissions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transmissions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transmissions);
        }

        // GET: Transmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transmissions = await _context.Transmissions.FindAsync(id);
            if (transmissions == null)
            {
                return NotFound();
            }
            return View(transmissions);
        }

        // POST: Transmissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransmissionId,Trasmission")] Transmissions transmissions)
        {
            if (id != transmissions.TransmissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transmissions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransmissionsExists(transmissions.TransmissionId))
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
            return View(transmissions);
        }

        // GET: Transmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transmissions = await _context.Transmissions
                .FirstOrDefaultAsync(m => m.TransmissionId == id);
            if (transmissions == null)
            {
                return NotFound();
            }

            return View(transmissions);
        }

        // POST: Transmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transmissions = await _context.Transmissions.FindAsync(id);
            _context.Transmissions.Remove(transmissions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransmissionsExists(int id)
        {
            return _context.Transmissions.Any(e => e.TransmissionId == id);
        }
    }
}
