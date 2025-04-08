using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UtazasSzervezo_Library;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.APIControllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly UtazasSzervezoDbContext _context;

        public StatisticsController(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        [HttpGet("bookings-per-month")]
        public IActionResult GetBookingsPerMonth()
        {
            var data = _context.Bookings
                .GroupBy(b => b.start_date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(data);
        }

        [HttpGet("revenue-per-month")]
        public IActionResult GetRevenuePerMonth()
        {
            var data = _context.Bookings
                .GroupBy(b => b.start_date.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Total = g.Sum(b => b.total_price)
                })
                .OrderBy(x => x.Month)
                .ToList();

            return Ok(data);
        }

        [HttpGet("popular-cities")]
        public IActionResult GetPopularCities()
        {
            var data = _context.Bookings
                .Include(b => b.Accommodation)
                .Where(b => b.Accommodation != null)
                .GroupBy(b => b.Accommodation.city)
                .Select(g => new
                {
                    City = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            return Ok(data);
        }
        //[HttpGet("bookings-per-month")]
        //public IActionResult GetBookingsPerMonth()
        //{
        //    var result = new[]
        //    {
        //        new { Month = 1, Count = 10 },
        //        new { Month = 2, Count = 8 },
        //        new { Month = 3, Count = 14 }
        //    };
        //    return Ok(result);
        //}

        //[HttpGet("revenue-per-month")]
        //public IActionResult GetRevenuePerMonth()
        //{
        //    var result = new[]
        //    {
        //        new { Month = 1, Revenue = 200000 },
        //        new { Month = 2, Revenue = 180000 },
        //        new { Month = 3, Revenue = 250000 }
        //    };
        //    return Ok(result);
        //}

        //[HttpGet("popular-cities")]
        //public IActionResult GetPopularCities()
        //{
        //    var result = new[]
        //    {
        //        new { City = "Budapest", Count = 15 },
        //        new { City = "London", Count = 12 },
        //        new { City = "Párizs", Count = 10 }
        //    };
        //    return Ok(result);
        //}
    }
}
