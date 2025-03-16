using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
{
    public class AmenityService
    {
        private readonly UtazasSzervezoDbContext _context;
        public AmenityService(UtazasSzervezoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Amenity>> GetAllAmenities()
        {
            return await _context.Amenities.ToListAsync();
        }

        public async Task<Amenity> GetAmenityById(int id)
        {
            return await _context.Amenities.FindAsync(id);
        }

        public async Task<Amenity> CreateAmenity(Amenity amenity)
        {
            if (amenity.AccommodationAmenities != null && !amenity.AccommodationAmenities.Any())
            {
                amenity.AccommodationAmenities = null;
            }

            _context.Amenities.Add(amenity);
            await _context.SaveChangesAsync();
            return amenity;
        }

        public async Task<bool> UpdateAmenity(int id, Amenity amenity)
        {
            var existing = await _context.Amenities.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.name = amenity.name;

            _context.Amenities.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAmenity(int id)
        {
            var amenities = await _context.Amenities.FindAsync(id);
            if (amenities == null) return false;

            _context.Amenities.Remove(amenities);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
