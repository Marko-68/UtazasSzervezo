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
            var acc = new Accommodation()
            {
                Name = accommodation.Name,
                Description = accommodation.Description,
                Type = accommodation.Type,
                Number_of_rooms = accommodation.Number_of_rooms,
                Max_person = accommodation.Max_person,
                Address = accommodation.Address,
                City = accommodation.City,
                Country = accommodation.Country,
                Price_per_night = accommodation.Price_per_night,
                Available_rooms = accommodation.Available_rooms,
                Dinning = accommodation.Dinning
            };

            _context.Accommodations.Add(accommodation);
            await _context.SaveChangesAsync();  
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
