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

        public async Task<Accommodation> CreateAccommodation(Accommodation accommodation)
        {
            _context.Accommodations.Add(accommodation);
            await _context.SaveChangesAsync();

            // Amenity-k hozzáadása
            if (accommodation.AccommodationAmenities != null && accommodation.AccommodationAmenities.Any())
            {
                foreach (var accommodationAmenity in accommodation.AccommodationAmenities)
                {
                    var exists = await _context.AccommodationsAmenities
                        .AnyAsync(aa => aa.accommodation_id == accommodation.id && aa.amenity_id == accommodationAmenity.amenity_id);

                    if (!exists)
                    {
                        accommodationAmenity.accommodation_id = accommodation.id;
                        _context.AccommodationsAmenities.Add(accommodationAmenity);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return accommodation;
        }

        public async Task<bool> UpdateAccommodation(int id, Accommodation accommodation)
        {
            var existing = await _context.Accommodations.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.name = accommodation.name;
            existing.description = accommodation.description;
            existing.type = accommodation.type;
            existing.number_of_rooms = accommodation.number_of_rooms;
            existing.max_person = accommodation.max_person;
            existing.address = accommodation.address;
            existing.city = accommodation.city;
            existing.country = accommodation.country;
            existing.price_per_night = accommodation.price_per_night;
            existing.available_rooms = accommodation.available_rooms;
            existing.dinning = accommodation.dinning;

            _context.Accommodations.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccommodation(int id)
        {
            var accommodation = await _context.Accommodations.FindAsync(id);
            if (accommodation == null) return false;

            _context.Accommodations.Remove(accommodation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
