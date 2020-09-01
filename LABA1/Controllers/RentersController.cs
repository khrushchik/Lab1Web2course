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
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace LABA1.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class RentersController : Controller
    {
        private readonly MyDBContext _context;

        public RentersController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Renters
        public async Task<IActionResult> Index(SortRenters sortOrder = SortRenters.NameAsc)
        {
            IQueryable<Renters> myDBContext = _context.Renters;
            ViewData["PassportSort"] = sortOrder == SortRenters.PassportAsc ? SortRenters.PassportDesc : SortRenters.PassportAsc;
            ViewData["PhoneSort"] = sortOrder == SortRenters.PhoneAsc ? SortRenters.PhoneDesc : SortRenters.PhoneAsc;
            ViewData["DriveExpSort"] = sortOrder == SortRenters.DriveExpAsc ? SortRenters.DriveExpDesc : SortRenters.DriveExpAsc;
            ViewData["AdressSort"] = sortOrder == SortRenters.AddressAsc ? SortRenters.AddressDesc : SortRenters.AddressAsc;
            ViewData["NameSort"] = sortOrder == SortRenters.NameAsc ? SortRenters.NameDesc : SortRenters.NameAsc;
            myDBContext = sortOrder switch
            {
                SortRenters.PassportAsc => myDBContext.OrderBy(s => s.Passport),
                SortRenters.PassportDesc => myDBContext.OrderByDescending(s => s.Passport),
                SortRenters.PhoneAsc => myDBContext.OrderBy(s => s.Phone),
                SortRenters.PhoneDesc => myDBContext.OrderByDescending(s => s.Phone),
                SortRenters.DriveExpAsc => myDBContext.OrderBy(s => s.DriveExperience),
                SortRenters.DriveExpDesc => myDBContext.OrderByDescending(s => s.DriveExperience),
                SortRenters.AddressAsc => myDBContext.OrderBy(s => s.Address),
                SortRenters.AddressDesc => myDBContext.OrderByDescending(s => s.Address),
                SortRenters.NameAsc => myDBContext.OrderBy(s => s.Name),
                SortRenters.NameDesc => myDBContext.OrderByDescending(s => s.Name),
                _ => myDBContext.OrderBy(s => s.Name),
            };
            return View(await myDBContext.AsNoTracking().ToListAsync());

            //return View(await _context.Renters.ToListAsync());
        }

        // GET: Renters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renters = await _context.Renters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renters == null)
            {
                return NotFound();
            }

            return View(renters);
        }

        // GET: Renters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Renters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Passport,Phone,DriveExperience,Address")] Renters renters)
        {
            if (ModelState.IsValid)
            {
                _context.Add(renters);
                await _context.SaveChangesAsync();
                return RedirectToAction("Create", "Contracts");
                //return RedirectToAction(nameof(Index));
            }
            return View(renters);
        }

        // GET: Renters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renters = await _context.Renters.FindAsync(id);
            if (renters == null)
            {
                return NotFound();
            }
            return View(renters);
        }

        // POST: Renters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Passport,Phone,DriveExperience,Address")] Renters renters)
        {
            if (id != renters.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(renters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentersExists(renters.Id))
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
            return View(renters);
        }

        // GET: Renters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var renters = await _context.Renters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (renters == null)
            {
                return NotFound();
            }

            return View(renters);
        }

        // POST: Renters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //new
            IQueryable<Contracts> contracts = from o in _context.Contracts where o.RenterId == id select o;
            Renters renters = _context.Renters.Find(id);
            foreach (Contracts contracts1 in contracts)
            {
                _context.Contracts.Remove(contracts1);
            }
            _context.Renters.Remove(renters);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //new

            /*var renters = await _context.Renters.FindAsync(id);
            _context.Renters.Remove(renters);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));*/
        }

        private bool RentersExists(int id)
        {
            return _context.Renters.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    Renters renters = new Renters();
                                    if (row.Cell(1).Value.ToString().Length > 0)
                                    {
                                        var a = (from rent in _context.Renters
                                                 where rent.Passport.Contains(row.Cell(1).Value.ToString())
                                                 select rent).ToList();                                        
                                        if (a.Count() > 0)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            renters.Passport = row.Cell(1).Value.ToString();
                                        }

                                    }
                                    renters.Phone = row.Cell(2).Value.ToString();
                                    renters.DriveExperience = row.Cell(3).Value.ToString();
                                    renters.Address = row.Cell(4).Value.ToString();
                                    renters.Name = row.Cell(5).Value.ToString();
                                    _context.Renters.Add(renters);
                                    if (row.Cell(1).Value.ToString().Length == 0 || row.Cell(2).Value.ToString().Length == 0 || row.Cell(3).Value.ToString().Length == 0 || row.Cell(4).Value.ToString().Length == 0 || row.Cell(5).Value.ToString().Length == 0)
                                    {
                                        return NotFound();
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var renters = _context.Renters.ToList();
                var worksheet = workbook.Worksheets.Add("Renters");
                foreach (var c in renters)
                {
                    worksheet.Cell("A1").Value = "Паспорт";
                    worksheet.Cell("B1").Value = "Телефон";
                    worksheet.Cell("C1").Value = "Стаж";
                    worksheet.Cell("D1").Value = "Адрес";
                    worksheet.Cell("E1").Value = "ФИО";
                    worksheet.Row(1).Style.Font.Bold = true;

                    for (int i = 0; i < renters.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = renters[i].Passport;
                        worksheet.Cell(i + 2, 2).Value = renters[i].Phone;
                        worksheet.Cell(i + 2, 3).Value = renters[i].DriveExperience;
                        worksheet.Cell(i + 2, 4).Value = renters[i].Address;
                        worksheet.Cell(i + 2, 5).Value = renters[i].Name;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"renters_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
