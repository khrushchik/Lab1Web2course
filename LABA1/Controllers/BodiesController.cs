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
    public class BodiesController : Controller
    {
        private readonly MyDBContext _context;

        public BodiesController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Bodies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bodies.ToListAsync());
        }

        // GET: Bodies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodies = await _context.Bodies
                .FirstOrDefaultAsync(m => m.BodyId == id);
            if (bodies == null)
            {
                return NotFound();
            }

            return View(bodies);
        }

        // GET: Bodies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bodies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BodyId,Body")] Bodies bodies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bodies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bodies);
        }

        // GET: Bodies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodies = await _context.Bodies.FindAsync(id);
            if (bodies == null)
            {
                return NotFound();
            }
            return View(bodies);
        }

        // POST: Bodies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BodyId,Body")] Bodies bodies)
        {
            if (id != bodies.BodyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bodies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodiesExists(bodies.BodyId))
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
            return View(bodies);
        }

        // GET: Bodies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodies = await _context.Bodies
                .FirstOrDefaultAsync(m => m.BodyId == id);
            if (bodies == null)
            {
                return NotFound();
            }

            return View(bodies);
        }

        // POST: Bodies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bodies = await _context.Bodies.FindAsync(id);
            _context.Bodies.Remove(bodies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodiesExists(int id)
        {
            return _context.Bodies.Any(e => e.BodyId == id);
        }
    }
}
