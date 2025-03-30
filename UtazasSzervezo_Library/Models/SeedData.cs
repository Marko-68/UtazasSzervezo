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
                            cover_img = "./images/Accommodation_img/budhotel1.jpg",
                            images_url = ["./images/Accommodation_img/acc1_1_images.jpg",
                                          "./images/Accommodation_img/acc1_2_images.jpg",
                                          "./images/Accommodation_img/acc1_3_images.jpg"]
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
                        },
                        new Accommodation
                        {
                            name = "Beach Resort",
                            description = "Luxury resort right on the beach with private access.",
                            type = "Resort",
                            number_of_rooms = 100,
                            max_person = 3,
                            address = "789 Coastal Avenue",
                            city = "Split",
                            country = "Croatia",
                            price_per_night = 300,
                            star_rating = 5,
                            available_rooms = 40,
                            dinning = "All-inclusive",
                            cover_img = "./images/Accommodation_img/beach-resort.jpg"
                        },
                        new Accommodation
                        {
                            name = "City Apartments",
                            description = "Modern apartments in the city center.",
                            type = "Apartment",
                            number_of_rooms = 30,
                            max_person = 2,
                            address = "101 Downtown Street",
                            city = "Berlin",
                            country = "Germany",
                            price_per_night = 120,
                            star_rating = 4,
                            available_rooms = 15,
                            dinning = "Self-catering",
                            cover_img = "./images/Accommodation_img/city-apartment.jpg"
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
                        },
                        new Amenity
                        {
                            name = "Air Conditioning"
                        },
                        new Amenity
                        {
                            name = "Fitness Center"
                        },
                        new Amenity
                        {
                            name = "Spa"
                        },
                        new Amenity
                        {
                            name = "Pet Friendly"
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
                        },
                        new Flight
                        {
                            airline = "Lufthansa",
                            departure_time = DateTime.Now.AddDays(5),
                            arrival_time = DateTime.Now.AddDays(5).AddHours(1.5),
                            departure_airport = "BUD",
                            destination_airport = "MUC",
                            duration = 90,
                            available_seats = 180,
                            price = 129
                        },
                        new Flight
                        {
                            airline = "British Airways",
                            departure_time = DateTime.Now.AddDays(8),
                            arrival_time = DateTime.Now.AddDays(8).AddHours(2.5),
                            departure_airport = "BUD",
                            destination_airport = "LHR",
                            duration = 150,
                            available_seats = 220,
                            price = 159
                        }
                    );
                }

                //if (!context.Bookings.Any())
                //{
                //    context.Bookings.AddRange(
                //        new Booking
                //        {
                //            accommodation_id = 1,
                //            flight_id = 1,
                //            start_date = DateTime.Now.AddDays(10),
                //            end_date = DateTime.Now.AddDays(15),
                //            description = "vacation",
                //            status = "Confirmed",
                //            special_request = "Late check-in",
                //            total_price = 749
                //        },
                //        new Booking
                //        {
                //            accommodation_id = 2,
                //            flight_id = 2,
                //            start_date = DateTime.Now.AddDays(20),
                //            end_date = DateTime.Now.AddDays(25),
                //            description = "Winter getaway",
                //            status = "Pending",
                //            special_request = "Extra blanket",
                //            total_price = 999
                //        }
                //    );
                //}

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
