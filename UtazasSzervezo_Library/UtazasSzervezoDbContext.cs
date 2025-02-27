using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library
{
    public class UtazasSzervezoDbContext : IdentityDbContext<User>
    {
        public UtazasSzervezoDbContext(DbContextOptions<UtazasSzervezoDbContext> options)
           : base(options) { }
        public DbSet<Accommodation> Accommodations { get; set; }
        public DbSet<AccommodationAmenities> AccommodationsAmenities { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Review> Reviews { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensures Identity Tables are configured properly

            // Example: Customize Identity table names if needed
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            modelBuilder.Entity<AccommodationAmenities>()
                .HasKey(aa => new { aa.Accommodation_id, aa.Amentry_id });

            modelBuilder.Entity<AccommodationAmenities>()
                .HasOne(aa => aa.Accommodation)
                .WithMany(a => a.AccommodationAmenities)
                .HasForeignKey(aa => aa.Accommodation_id);

            modelBuilder.Entity<AccommodationAmenities>()
                .HasOne(aa => aa.Amenity)
                .WithMany(a => a.AccommodationAmenities)
                .HasForeignKey(aa => aa.Amentry_id);
        }

    }
}
