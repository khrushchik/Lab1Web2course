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
    public class ColorsController : Controller
    {
        private readonly MyDBContext _context;

        public ColorsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Colors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colors.ToListAsync());
        }

        // GET: Colors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorId == id);
            if (colors == null)
            {
                return NotFound();
            }

            return View(colors);
        }

        // GET: Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Colors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColorId,Color")] Colors colors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colors);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colors);
        }

        // GET: Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }
            return View(colors);
        }

        // POST: Colors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColorId,Color")] Colors colors)
        {
            if (id != colors.ColorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorsExists(colors.ColorId))
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
            return View(colors);
        }

        // GET: Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colors = await _context.Colors
                .FirstOrDefaultAsync(m => m.ColorId == id);
            if (colors == null)
            {
                return NotFound();
            }

            return View(colors);
        }

        // POST: Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colors = await _context.Colors.FindAsync(id);
            _context.Colors.Remove(colors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorsExists(int id)
        {
            return _context.Colors.Any(e => e.ColorId == id);
        }
    }
}
