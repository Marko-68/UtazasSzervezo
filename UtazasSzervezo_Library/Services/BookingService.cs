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
            return await _context.Bookings
                .Include(b => b.Accommodation)
                .Include(b => b.Flight)
                .ToListAsync();
        }

        public async Task<Booking> GetBookingById(int id)
        {
            return await _context.Bookings
                .Include(b => b.Accommodation)
                .Include(b => b.Flight)
                .FirstOrDefaultAsync(b => b.id == id);
        }

        public async Task<Booking> CreateBooking(Booking booking)
        {
            if (booking.flight_id != null)
            {
                var flight = await _context.Flights.FindAsync(booking.flight_id);
                if (flight != null && flight.available_seats > 0)
                {
                    flight.available_seats -= 1;
                    booking.Flight = flight;
                }
                else
                {
                    throw new InvalidOperationException("No available seats left.");
                }
            }

            if (booking.accommodation_id != null)
            {
                var accommodation = await _context.Accommodations.FindAsync(booking.accommodation_id);
                if (accommodation != null && accommodation.available_rooms > 0)
                {
                    accommodation.available_rooms -= 1;
                    booking.Accommodation = accommodation;
                }
                else
                {
                    throw new InvalidOperationException("No available rooms left.");
                }
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
