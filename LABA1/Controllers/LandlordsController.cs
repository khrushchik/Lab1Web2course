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
    public class LandlordsController : Controller
    {
        private readonly MyDBContext _context;

        public LandlordsController(MyDBContext context)
        {
            _context = context;
        }

        // GET: Landlords
        public async Task<IActionResult> Index(SortLandlords sortOrder=SortLandlords.NameAsc)
        {
            //var myDBContext = _context.Landlords.Include(l => l.Country);
            IQueryable<Landlords> myDBContext = _context.Landlords.Include(l => l.Country);
            ViewData["NameSort"] = sortOrder == SortLandlords.NameAsc ? SortLandlords.NameDesc: SortLandlords.NameAsc;
            ViewData["CountrySort"] = sortOrder == SortLandlords.CountryAsc? SortLandlords.CountryDesc : SortLandlords.CountryAsc;
            ViewData["ContactPersonSort"] = sortOrder == SortLandlords.ContactPersonAsc? SortLandlords.ContactPersonDesc : SortLandlords.ContactPersonAsc;
            ViewData["PhoneSort"] = sortOrder == SortLandlords.PhoneAsc? SortLandlords.PhoneDesc: SortLandlords.PhoneAsc;
            myDBContext = sortOrder switch
            {
                SortLandlords.NameAsc => myDBContext.OrderBy(s => s.Name),
                SortLandlords.NameDesc => myDBContext.OrderByDescending(s => s.Name),
                SortLandlords.CountryAsc => myDBContext.OrderBy(s => s.Country),
                SortLandlords.CountryDesc => myDBContext.OrderByDescending(s => s.Country),
                SortLandlords.ContactPersonAsc => myDBContext.OrderBy(s => s.ContartPerson),
                SortLandlords.ContactPersonDesc => myDBContext.OrderByDescending(s => s.ContartPerson),
                SortLandlords.PhoneAsc => myDBContext.OrderBy(s => s.Phone),
                SortLandlords.PhoneDesc => myDBContext.OrderByDescending(s => s.Phone),
                _=>myDBContext.OrderBy(s=>s.Name),
            };
            return View(await myDBContext.AsNoTracking().ToListAsync());
            //return View(await myDBContext.ToListAsync());
        }

        // GET: Landlords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var landlords = await _context.Landlords
                .Include(l => l.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (landlords == null)
            {
                return NotFound();
            }

            return View(landlords);
            //return RedirectToAction("Index", "Cars", new { id = landlords.Id, name = landlords.Name });//new
        }

        // GET: Landlords/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "Country");
            return View();
        }

        // POST: Landlords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryId,ContartPerson,Phone")] Landlords landlords)
        {
            if (ModelState.IsValid)
            {
                _context.Add(landlords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "Country", landlords.CountryId);
            return View(landlords);
        }

        // GET: Landlords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var landlords = await _context.Landlords.FindAsync(id);
            if (landlords == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "Country", landlords.CountryId);
            return View(landlords);
        }

        // POST: Landlords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryId,ContartPerson,Phone")] Landlords landlords)
        {
            if (id != landlords.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(landlords);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LandlordsExists(landlords.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "Country", landlords.CountryId);
            return View(landlords);
        }

        // GET: Landlords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var landlords = await _context.Landlords
                .Include(l => l.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (landlords == null)
            {
                return NotFound();
            }

            return View(landlords);
        }

        // POST: Landlords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //new
            /*
                IQueryable<Cars> cars = from o in _context.Cars where o.LandlordId == id select o;
                Landlords landlords = _context.Landlords.Find(id);
                foreach (Cars cars1 in cars)
                {
                    _context.Cars.Remove(cars1);
                }
                _context.Landlords.Remove(landlords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));*/
            //new

            var landlords = await _context.Landlords.FindAsync(id);
             var cars = from c in _context.Cars where c.LandlordId==id select c;
             foreach(var c in cars)
             {
                var contracts = from co in _context.Contracts where co.CarId == c.Id select co;
                foreach(var co in contracts)
                {
                    _context.Contracts.Remove(co);
                }
                _context.Cars.Remove(c);
             }
            _context.Landlords.Remove(landlords);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool LandlordsExists(int id)
        {
            return _context.Landlords.Any(e => e.Id == id);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
       /* public async Task<IActionResult> Import(IFormFile fileExcel)
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
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Landlords newland;
                                var c = (from land in _context.Landlords
                                         where land.Name.Contains(worksheet.Name)
                                         select land).ToList();
                                if (c.Count > 0)
                                {
                                    newland = c[0];
                                }
                                else
                                {
                                    newland = new Landlords();
                                    newland.Name = worksheet.Name;
                                    newland.ContartPerson = "Проверка";
                                    newland.CountryId = 1;
                                    newland.Phone = "13042020";
                                    // xz newland.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Landlords.Add(newland);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                        Cars car = new Cars();
                                        //car.Label.Lable = row.Cell(1).Value.ToString();
                                        if (row.Cell(1).Value.ToString().Length > 0)
                                        {
                                            Labels labels;
                                            var a = (from aut in _context.Labels
                                                     where aut.Lable.Contains(row.Cell(1).Value.ToString())
                                                     select aut).ToList();
                                            if (a.Count() > 0)
                                            {
                                                labels = a[0];
                                            }
                                            else
                                            {
                                                labels = new Labels
                                                {
                                                    Lable = row.Cell(1).Value.ToString()
                                                };
                                                _context.Add(labels);
                                            }
                                            car.Label = labels;
                                        }
                                        //car.Body.Body = row.Cell(2).Value.ToString();
                                        if (row.Cell(2).Value.ToString().Length > 0)
                                        {
                                            Bodies bodies;
                                            var a = (from aut in _context.Bodies
                                                     where aut.Body.Contains(row.Cell(2).Value.ToString())
                                                     select aut).ToList();
                                            if (a.Count() > 0)
                                            {
                                                bodies = a[0];
                                            }
                                            else
                                            {
                                                bodies = new Bodies()
                                                {
                                                    Body = row.Cell(2).Value.ToString()
                                                };
                                                _context.Add(bodies);
                                            }
                                            car.Body = bodies;
                                        }
                                        //car.Color.Color= row.Cell(3).Value.ToString();
                                        if (row.Cell(3).Value.ToString().Length > 0)
                                        {
                                            Colors colors;
                                            var a = (from aut in _context.Colors
                                                     where aut.Color.Contains(row.Cell(3).Value.ToString())
                                                     select aut).ToList();
                                            if (a.Count() > 0)
                                            {
                                                colors = a[0];
                                            }
                                            else
                                            {
                                                colors = new Colors
                                                {
                                                    Color = row.Cell(3).Value.ToString()
                                                };
                                                _context.Add(colors);
                                            }
                                            car.Color = colors;
                                        }
                                        car.Year= Convert.ToInt32(row.Cell(4).Value);
                                        //car.Transmission.Trasmission= row.Cell(5).Value.ToString();
                                        if (row.Cell(5).Value.ToString().Length > 0)
                                        {
                                            Transmissions transmissions;
                                            var a = (from aut in _context.Transmissions
                                                     where aut.Trasmission.Contains(row.Cell(5).Value.ToString())
                                                     select aut).ToList();
                                            if (a.Count() > 0)
                                            {
                                                transmissions = a[0];
                                            }
                                            else
                                            {
                                                transmissions = new Transmissions
                                                {
                                                    Trasmission = row.Cell(5).Value.ToString()
                                                };
                                                _context.Add(transmissions);
                                            }
                                            car.Transmission = transmissions;
                                        }
                                        car.Price= Convert.ToInt32(row.Cell(6).Value);
                                        car.Landlord = newland;
                                        car.GovNumber = row.Cell(8).Value.ToString();
                                        _context.Cars.Add(car);
                                    if(row.Cell(5).Value.ToString().Length==0|| row.Cell(3).Value.ToString().Length==0|| row.Cell(2).Value.ToString().Length==0|| row.Cell(1).Value.ToString().Length == 0)
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
        }*/
        /*public ActionResult Export(int?id)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                //var landlords = _context.Landlords.Include("Cars").ToList();
                string landlords = _context.Landlords.FirstOrDefault(m => m.Id == id).Name;
                var worksheet = workbook.Worksheets.Add(landlords);
                foreach (var c in landlords)
                {
                    worksheet.Cell("A1").Value = "Марка";
                    worksheet.Cell("B1").Value = "Кузов";
                    worksheet.Cell("C1").Value = "Цвет";
                    worksheet.Cell("D1").Value = "Год";
                    worksheet.Cell("E1").Value = "Трансмиссия";
                    worksheet.Cell("F1").Value = "Цена";
                    worksheet.Cell("G1").Value = "Арендодатель";
                    worksheet.Cell("H1").Value = "Гос номер";
                    worksheet.Row(1).Style.Font.Bold = true;
                    //var books = c.Books.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < landlords.Count(); i++)
                    {
                        var car = _context.Cars.Where(f => f.Id == i).Include(f => f.Label.Lable).Include(f => f.Body.Body).Include(f => f.Color.Color).Include(f => f.Year).Include(f => f.Transmission.Trasmission).Include(f => f.Price).Include(f => f.Landlord.Name).Include(f => f.GovNumber);
                        worksheet.Cell(i+2,1).Value=car.

                    }
                }
                using (var stream = new MemoryStream())
                {   
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }*/
       /* public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var landlords = _context.Landlords.Include("Cars").ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проектах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var c in landlords)
                {
                    var worksheet = workbook.Worksheets.Add(c.Name);

                    worksheet.Cell("A1").Value = "Марка";
                    worksheet.Cell("B1").Value = "Кузов";
                    worksheet.Cell("C1").Value = "Цвет";
                    worksheet.Cell("D1").Value = "Год";
                    worksheet.Cell("E1").Value = "Трансмиссия";
                    worksheet.Cell("F1").Value = "Цена";
                    worksheet.Cell("G1").Value = "Арендодатель";
                    worksheet.Cell("H1").Value = "Гос номер";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var cars = c.Cars.ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < cars.Count; i++)
                    {
                        //cars = _context.Cars.Where(f => f.Id == cars[i].Id).Include(f => f.Label.Lable).Include(f => f.Body.Body).Include(f => f.Color.Color).Include(f => f.Year).Include(f => f.Transmission.Trasmission).Include(f => f.Price).Include(f => f.Landlord.Name).Include(f => f.GovNumber).ToList();
                        worksheet.Cell(i + 2, 1).Value = cars[i].Label;
                        worksheet.Cell(i + 2, 2).Value = cars[i].Body;
                        worksheet.Cell(i + 2, 3).Value = cars[i].Color;
                        worksheet.Cell(i + 2, 4).Value = cars[i].Year;
                        worksheet.Cell(i + 2, 5).Value = cars[i].Transmission;
                        worksheet.Cell(i + 2, 6).Value = cars[i].Price;
                        worksheet.Cell(i + 2, 7).Value = cars[i].Landlord;
                        worksheet.Cell(i + 2, 8).Value = cars[i].GovNumber;

                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }*/
    }

}
