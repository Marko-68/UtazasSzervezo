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
                .Where(b => b.start_date != null)
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
                .Where(b => b.start_date != null)
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
                .Where(b => b.Accommodation != null && b.Accommodation.city != null)
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