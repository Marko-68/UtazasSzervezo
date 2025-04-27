using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library;
using Microsoft.EntityFrameworkCore;
using System.Net;

public class AccommodationService
{
    private readonly UtazasSzervezoDbContext _context;

    public AccommodationService(UtazasSzervezoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Accommodation>> GetAllAccommodations()
    {
        return await _context.Accommodations
            .Include(a => a.AccommodationAmenities)
            .ThenInclude(aa => aa.Amenity)
            .ToListAsync();
    }

    public async Task<Accommodation?> GetAccommodationById(int id)
    {
        return await _context.Accommodations
            .Include(a => a.AccommodationAmenities)
            .ThenInclude(aa => aa.Amenity)
            .FirstOrDefaultAsync(a => a.id == id);

    }

    public async Task<Accommodation> CreateAccommodation(Accommodation accommodation)
    {
        _context.Accommodations.Add(accommodation);
        await _context.SaveChangesAsync();

        if (accommodation.AccommodationAmenities != null && accommodation.AccommodationAmenities.Any())
        {
            foreach (var accommodationAmenity in accommodation.AccommodationAmenities)
            {
                var exists = await _context.AccommodationsAmenities
                    .AnyAsync(aa => aa.accommodation_id == accommodation.id &&
                                   aa.amenity_id == accommodationAmenity.amenity_id);

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
        var existing = await _context.Accommodations
            .Include(a => a.AccommodationAmenities)
            .FirstOrDefaultAsync(a => a.id == id);

        if (existing == null) return false;

        //alapvető tulajdonságok frissítése
        _context.Entry(existing).CurrentValues.SetValues(accommodation);

        //Amenitties
        if (accommodation.AccommodationAmenities != null)
        {
            foreach (var existingAmenity in existing.AccommodationAmenities.ToList())
            {
                if (!accommodation.AccommodationAmenities.Any(a => a.amenity_id == existingAmenity.amenity_id))
                {
                    _context.AccommodationsAmenities.Remove(existingAmenity);
                }
            }

            foreach (var newAmenity in accommodation.AccommodationAmenities)
            {
                if (!existing.AccommodationAmenities.Any(a => a.amenity_id == newAmenity.amenity_id))
                {
                    existing.AccommodationAmenities.Add(new AccommodationAmenities
                    {
                        accommodation_id = id,
                        amenity_id = newAmenity.amenity_id
                    });
                }
            }
        }

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

    public async Task<bool> ExistAtAddress(string address)
    {
        return await _context.Accommodations.AnyAsync(a => a.address == address);
    }
}