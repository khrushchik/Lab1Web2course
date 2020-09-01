using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LABA1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly MyDBContext _context;
        public ChartsController(MyDBContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var landlords = _context.Landlords.Include(b => b.Cars).ToList();
            List<object> landCar = new List<object>();
            landCar.Add(new[] { "Арендодатель", "Количество машин" });
            foreach(var c in landlords)
            {
                landCar.Add(new object[] { c.Name, c.Cars.Count() });
            }
            return new JsonResult(landCar);
        }
        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var transmissions = _context.Transmissions.Include(b => b.Cars).ToList();
            List<object> transCar = new List<object>();
            transCar.Add(new[] { "Коробка передач", "Количество машин" });
            foreach (var c in transmissions)
            {
                transCar.Add(new object[] { c.Trasmission, c.Cars.Count() });
            }
            return new JsonResult(transCar);
        }

    }
}