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
    public class LabelsController : Controller
    {
        private readonly MyDBContext _context;

        public LabelsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Labels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Labels.ToListAsync());
        }

        // GET: Labels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels
                .FirstOrDefaultAsync(m => m.LableId == id);
            if (labels == null)
            {
                return NotFound();
            }

            return View(labels);
        }

        // GET: Labels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LableId,Lable")] Labels labels)
        {
            if (ModelState.IsValid)
            {
                _context.Add(labels);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(labels);
        }

        // GET: Labels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels.FindAsync(id);
            if (labels == null)
            {
                return NotFound();
            }
            return View(labels);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LableId,Lable")] Labels labels)
        {
            if (id != labels.LableId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(labels);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelsExists(labels.LableId))
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
            return View(labels);
        }

        // GET: Labels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var labels = await _context.Labels
                .FirstOrDefaultAsync(m => m.LableId == id);
            if (labels == null)
            {
                return NotFound();
            }

            return View(labels);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var labels = await _context.Labels.FindAsync(id);
            _context.Labels.Remove(labels);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabelsExists(int id)
        {
            return _context.Labels.Any(e => e.LableId == id);
        }
    }
}
