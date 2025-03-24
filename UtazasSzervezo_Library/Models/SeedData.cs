using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtazasSzervezo_Library.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<UtazasSzervezoDbContext>();
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Accommodations.Any())
                {
                    context.Accommodations.AddRange(
                        new Accommodation
                        {
                            name = "Hotel Sunshine",
                            description = "A beautiful hotel with a great view.",
                            type = "Hotel",
                            number_of_rooms = 50,
                            max_person = 2,
                            address = "123 Sunshine Street",
                            city = "Budapest",
                            country = "Hungary",
                            price_per_night = 150,
                            star_rating = 4,
                            available_rooms = 25,
                            dinning = "Breakfast",
                            cover_img = "./images/Accommodation_img/budhotel1.jpg"
                        },
                        new Accommodation
                        {
                            name = "Mountain Lodge",
                            description = "A cozy lodge in the mountains.",
                            type = "Lodge",
                            number_of_rooms = 20,
                            max_person = 4,
                            address = "456 Mountain Road",
                            city = "Zakopane",
                            country = "Poland",
                            price_per_night = 200,
                            star_rating = 3,
                            available_rooms = 10,
                            dinning = "Half-board",
                            cover_img = "./images/Accommodation_img/aparthotel-gievont-boutique.jpg"
                        }
                    );
                }
                //if (!context.AccommodationsAmenities.Any())
                //{
                //    context.AccommodationsAmenities.AddRange(
                //        new AccommodationAmenities
                //        {
                //            accommodation_id = 1,
                //            amenity_id = 1
                //        },
                //        new AccommodationAmenities
                //        {
                //            accommodation_id = 1,
                //            amenity_id = 2
                //        }
                //    );
                //}
                if (!context.Amenities.Any())
                {
                    context.Amenities.AddRange(
                        new Amenity
                        {
                            name = "Free Wi-Fi"
                        },
                        new Amenity
                        {
                            name = "Swimming Pool"
                        }
                    );
                }

                if (!context.Flights.Any())
                {
                    context.Flights.AddRange(
                        new Flight
                        {
                            airline = "Wizz Air",
                            departure_time = DateTime.Now.AddDays(10),
                            arrival_time = DateTime.Now.AddDays(10).AddHours(2),
                            departure_airport = "BUD",
                            destination_airport = "VIE",
                            duration = 120,
                            available_seats = 150,
                            price = 59
                        },
                        new Flight
                        {
                            airline = "Ryanair",
                            departure_time = DateTime.Now.AddDays(15),
                            arrival_time = DateTime.Now.AddDays(15).AddHours(3),
                            departure_airport = "BUD",
                            destination_airport = "BCN",
                            duration = 180,
                            available_seats = 200,
                            price = 79
                        }
                    );
                }

                if (!context.Bookings.Any())
                {
                    context.Bookings.AddRange(
                        new Booking
                        {
                            accommodation_id = 1,
                            flight_id = 1,
                            start_date = DateTime.Now.AddDays(10),
                            end_date = DateTime.Now.AddDays(15),
                            description = "vacation",
                            status = "Confirmed",
                            special_request = "Late check-in",
                            total_price = 749
                        },
                        new Booking
                        {
                            accommodation_id = 2,
                            flight_id = 2,
                            start_date = DateTime.Now.AddDays(20),
                            end_date = DateTime.Now.AddDays(25),
                            description = "Winter getaway",
                            status = "Pending",
                            special_request = "Extra blanket",
                            total_price = 999
                        }
                    );
                }

                //if (!context.Reviews.Any())
                //{
                //    context.Reviews.AddRange(
                //        new Review
                //        {
                //            user_id = 1,
                //            accommodation_id = 1,
                //            rating = 9,
                //            comment = "Great experience!",
                //            created_at = DateTime.Now
                //        },
                //        new Review
                //        {
                //            user_id = 1,
                //            flight_id = 1,
                //            rating = 8,
                //            comment = "Comfortable flight.",
                //            created_at = DateTime.Now
                //        }
                //    );
                //}

                context.SaveChanges();
            }
        }
    }
}
