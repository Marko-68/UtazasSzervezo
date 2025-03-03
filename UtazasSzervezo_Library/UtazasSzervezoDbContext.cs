using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");

            //relationships
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "server=localhost;database=UtazasSzervezoIdentityDB;user=root;password=;";
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                    b => b.MigrationsAssembly("UtazasSzervezo_API"));
            }
        }

    }
}
