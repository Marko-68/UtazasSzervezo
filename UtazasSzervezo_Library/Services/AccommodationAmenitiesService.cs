using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class AccommodationAmenitiesService
    {
        private readonly UtazasSzervezoDbContext _context;

        public AccommodationAmenitiesService(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<IEnumerable<AccommodationAmenities>> GetAllAmenities()
        {
            return await _context.AccommodationsAmenities
                .Include(a => a.Accommodation)
                .Include(a => a.Amenity)
                .ToListAsync();
        }

        // GET BY ID
        public async Task<AccommodationAmenities> GetAmenityById(int id)
        {
            return await _context.AccommodationsAmenities
                .Include(a => a.Accommodation)
                .Include(a => a.Amenity)
                .FirstOrDefaultAsync(a => a.id == id);
        }

        // CREATE
        public async Task<AccommodationAmenities> CreateAmenity(AccommodationAmenities amenity)
        {
            _context.AccommodationsAmenities.Add(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        // DELETE
        public async Task<bool> DeleteAmenity(int id)
        {
            var amenity = await _context.AccommodationsAmenities.FindAsync(id);
            if (amenity == null) return false;

            _context.AccommodationsAmenities.Remove(amenity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
