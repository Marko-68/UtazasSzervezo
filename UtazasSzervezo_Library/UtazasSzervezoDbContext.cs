using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library
{
    public class UtazasSzervezoDbContext : IdentityDbContext<IdentityUser>
    {
        public UtazasSzervezoDbContext(DbContextOptions<UtazasSzervezoDbContext> options)
           : base(options) { }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<AccommodationAmenities> AccommodationsAmenities { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
