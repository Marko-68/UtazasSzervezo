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
