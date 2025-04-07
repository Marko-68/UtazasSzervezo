using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UtazasSzervezo_Library;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.APIControllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticsController : Controller
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
    }
}
