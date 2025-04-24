using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using UtazasSzervezo_Library;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.APIControllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticController : ControllerBase
    {
        private readonly UtazasSzervezoDbContext _context;

        public StatisticController(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        [HttpGet("popular-destinations")]
        public IActionResult GetPopularDestinations()
        {
            var popular = _context.Bookings
                .GroupBy(b => b.Destination.Name)
                .Select(g => new
                {
                    Destination = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            return Ok(popular);
        }

        [HttpGet("monthly-income")]
        public IActionResult GetMonthlyIncome()
        {
            var incomes = _context.Bookings
                .GroupBy(b => new { b.Date.Year, b.Date.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Income = g.Sum(b => b.TotalPrice)
                })
                .OrderBy(g => g.Month)
                .ToList();

            return Ok(incomes);
        }
    }
}
