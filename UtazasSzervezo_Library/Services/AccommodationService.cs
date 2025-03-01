using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class AccommodationService
    {
        private readonly UtazasSzervezoDbContext _context;
        public AccommodationService(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Accommodation>> GetAllAccommodations()
        {
            return await _context.Accommodations.ToListAsync();
        }

        public async Task<Accommodation> GetAccommodationById(int id)
        {
            return await _context.Accommodations.FindAsync(id);
        }

        public async Task CreateAccommodation(Accommodation accommodation)
        {
            _context.Accommodations.Add(accommodation);
            await _context.SaveChangesAsync();  
        }

    }
}
