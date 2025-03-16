using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class BookingService
    {
        private readonly UtazasSzervezoDbContext _context;
        public BookingService(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        public async Task<Booking> GetBookingById(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            if (booking.Accommodation != null)
            {
                _context.Accommodations.Add(booking.Accommodation);
            }
            if (booking.Flight != null)
            {
                _context.Flights.Add(booking.Flight);
            }

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<bool> UpdateBooking(int id, Booking booking)
        {
            var existing = await _context.Bookings.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.description = booking.description;
            existing.start_date = booking.start_date;
            existing.end_date = booking.end_date;
            existing.total_price = booking.total_price;

            _context.Bookings.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
