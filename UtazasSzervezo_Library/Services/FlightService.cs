using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class FlightService
    {
        private readonly UtazasSzervezoDbContext _context;
        public FlightService(UtazasSzervezoDbContext context)
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

        public async Task CreateFlight(Flight flight)
        {
            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateFlight(int id, Flight flight)
        {
            var existing = await _context.Flights.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.airline = flight.airline;
            existing.price = flight.price;
            existing.departure_time = flight.departure_time;
            existing.arrival_time = flight.arrival_time;
            existing.departure_airport = flight.departure_airport;
            existing.destination_airport = flight.destination_airport;
            existing.available_seats = flight.available_seats;
            existing.duration = flight.duration;

            _context.Flights.Update(existing);
            await _context.SaveChangesAsync();
            return true;
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
