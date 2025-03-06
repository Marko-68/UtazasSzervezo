using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class FlighService
    {
        private readonly UtazasSzervezoDbContext _context;
        public FlighService(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Flight>> GetAllFlights()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<Flight> GetFlightById(int id)
        {
            return await _context.Flights.FindAsync(id);
        }


        public async Task<bool> DeleteFlight(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null) return false;

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
