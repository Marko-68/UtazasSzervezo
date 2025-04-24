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
                            guests = 2,
                            address = "123 Sunshine Street",
                            city = "Budapest",
                            country = "Hungary",
                            price_per_night = 150,
                            star_rating = 4,
                            available_rooms = 25,
                            dinning = "Breakfast",
                            cover_img = "./images/Accommodation_img/budhotel1.jpg",
                            images_url = ["/images/Accommodation_img/acc1_1_images.jpg",
                        "/images/Accommodation_img/acc1_2_images.jpg",
                        "/images/Accommodation_img/acc1_3_images.jpg"]
                        },
                        new Accommodation
                        {
                            name = "Mountain Lodge",
                            description = "A cozy lodge in the mountains.",
                            type = "Lodge",
                            number_of_rooms = 20,
                            guests = 4,
                            address = "456 Mountain Road",
                            city = "Zakopane",
                            country = "Poland",
                            price_per_night = 200,
                            star_rating = 3,
                            available_rooms = 10,
                            dinning = "Half-board",
                            cover_img = "./images/Accommodation_img/aparthotel-gievont-boutique.jpg",
                            images_url = ["/images/Accommodation_img/acc2_1_images.jpg",
                        "/images/Accommodation_img/acc2_2_images.jpg",
                        "/images/Accommodation_img/acc3_3_images.jpg"]
                        },
                        new Accommodation
                        {
                            name = "Beach Resort",
                            description = "Luxury resort right on the beach with private access.",
                            type = "Resort",
                            number_of_rooms = 100,
                            guests = 3,
                            address = "789 Coastal Avenue",
                            city = "Split",
                            country = "Croatia",
                            price_per_night = 300,
                            star_rating = 5,
                            available_rooms = 40,
                            dinning = "All-inclusive",
                            cover_img = "./images/Accommodation_img/acc3_cover.jpg",
                            images_url = ["/images/Accommodation_img/acc3_cover.jpg",
                        "/images/Accommodation_img/acc_3_1_images.jpg",
                        "/images/Accommodation_img/acc_3_2_images.jpg"]
                        },
                        new Accommodation
                        {
                            name = "Costa del Sol Beach Hotel",
                            description = "Elegant beachfront hotel with stunning Mediterranean views.",
                            type = "Hotel",
                            number_of_rooms = 80,
                            guests = 2,
                            address = "101 Beachfront Promenade",
                            city = "Málaga",
                            country = "Spain",
                            price_per_night = 220,
                            star_rating = 4,
                            available_rooms = 35,
                            dinning = "Half-board",
                            cover_img = "./images/Accommodation_img/spain-hotel.jpg",
                            images_url = ["/images/Accommodation_img/spain1.jpg",
                                            "/images/Accommodation_img/spain2.jpg",
                                            "/images/Accommodation_img/spain3.jpg"]
                        },
                        new Accommodation
                        {
                            name = "Amalfi Coast Luxury Resort",
                            description = "Exclusive resort perched on the cliffs of Amalfi Coast.",
                            type = "Resort",
                            number_of_rooms = 60,
                            guests = 2,
                            address = "202 Cliffside Drive",
                            city = "Positano",
                            country = "Italy",
                            price_per_night = 350,
                            star_rating = 5,
                            available_rooms = 18,
                            dinning = "Breakfast",
                            cover_img = "./images/Accommodation_img/italy-hotel.jpg",
                            images_url = ["/images/Accommodation_img/italy1.jpg",
                                            "/images/Accommodation_img/italy2.jpg",
                                            "/images/Accommodation_img/italy3.jpg"]
                        },
                        new Accommodation
                        {
                            name = "French Riviera Boutique Hotel",
                            description = "Charming boutique hotel with private beach access.",
                            type = "Hotel",
                            number_of_rooms = 25,
                            guests = 2,
                            address = "303 Riviera Boulevard",
                            city = "Nice",
                            country = "France",
                            price_per_night = 280,
                            star_rating = 4,
                            available_rooms = 12,
                            dinning = "Breakfast",
                            cover_img = "./images/Accommodation_img/france-hotel.jpg",
                            images_url = ["/images/Accommodation_img/france1.jpg",
                                            "/images/Accommodation_img/france2.jpg",
                                            "/images/Accommodation_img/france3.jpg"]
                        },
                        new Accommodation
                        {
                            name = "Alpine Chalet",
                            description = "Traditional wooden chalet with mountain views.",
                            type = "Chalet",
                            number_of_rooms = 8,
                            guests = 6,
                            address = "202 Alpine Way",
                            city = "Innsbruck",
                            country = "Austria",
                            price_per_night = 250,
                            star_rating = 4,
                            available_rooms = 3,
                            dinning = "Self-catering",
                            cover_img = "./images/Accommodation_img/alpine-chalet.jpg",
                            images_url = ["/images/Accommodation_img/chalet1.jpg",
                        "/images/Accommodation_img/chalet2.jpg",
                        "/images/Accommodation_img/chalet3.jpg"]
                        },
                        new Accommodation
                        {
                            name = "Boutique Hotel",
                            description = "Charming boutique hotel with unique design.",
                            type = "Hotel",
                            number_of_rooms = 15,
                            guests = 2,
                            address = "303 Artistic Lane",
                            city = "Prague",
                            country = "Czech Republic",
                            price_per_night = 180,
                            star_rating = 4,
                            available_rooms = 7,
                            dinning = "Breakfast",
                            cover_img = "./images/Accommodation_img/boutique-hotel.jpg",
                            images_url = ["/images/Accommodation_img/boutique1.jpg",
                                        "/images/Accommodation_img/boutique2.jpg",
                                        "/images/Accommodation_img/boutique3.jpg"]
                        }
                    );
                }
                context.SaveChanges();

                if (!context.Amenities.Any())
                {
                    context.Amenities.AddRange(
                        new Amenity { name = "Free Wi-Fi" },
                        new Amenity { name = "Swimming Pool" },
                        new Amenity { name = "Air Conditioning" },
                        new Amenity { name = "Fitness Center" },
                        new Amenity { name = "Spa" },
                        new Amenity { name = "Pet Friendly" },
                        new Amenity { name = "Private Beach" },
                        new Amenity { name = "Sea View" }
                    );
                }
                context.SaveChanges();

                if (!context.AccommodationsAmenities.Any())
                {
                    context.AccommodationsAmenities.AddRange(
                        new AccommodationAmenities { accommodation_id = 1, amenity_id = 1 },
                        new AccommodationAmenities { accommodation_id = 1, amenity_id = 2 },
                        new AccommodationAmenities { accommodation_id = 1, amenity_id = 3 },
                        new AccommodationAmenities { accommodation_id = 2, amenity_id = 5 },
                        new AccommodationAmenities { accommodation_id = 2, amenity_id = 2 },
                        new AccommodationAmenities { accommodation_id = 2, amenity_id = 6 },
                        new AccommodationAmenities { accommodation_id = 3, amenity_id = 6 },
                        new AccommodationAmenities { accommodation_id = 3, amenity_id = 1 },
                        new AccommodationAmenities { accommodation_id = 3, amenity_id = 4 },
                        new AccommodationAmenities { accommodation_id = 4, amenity_id = 1 },
                        new AccommodationAmenities { accommodation_id = 4, amenity_id = 2 },
                        new AccommodationAmenities { accommodation_id = 4, amenity_id = 7 },
                        new AccommodationAmenities { accommodation_id = 4, amenity_id = 8 },
                        new AccommodationAmenities { accommodation_id = 5, amenity_id = 1 },
                        new AccommodationAmenities { accommodation_id = 5, amenity_id = 5 },
                        new AccommodationAmenities { accommodation_id = 5, amenity_id = 8 },
                        new AccommodationAmenities { accommodation_id = 6, amenity_id = 1 },
                        new AccommodationAmenities { accommodation_id = 6, amenity_id = 7 },
                        new AccommodationAmenities { accommodation_id = 6, amenity_id = 8 },
                        new AccommodationAmenities { accommodation_id = 7, amenity_id = 6 }, 
                        new AccommodationAmenities { accommodation_id = 7, amenity_id = 5 }, 
                        new AccommodationAmenities { accommodation_id = 8, amenity_id = 1 }, 
                        new AccommodationAmenities { accommodation_id = 8, amenity_id = 3 }, 
                        new AccommodationAmenities { accommodation_id = 8, amenity_id = 5 }  
                    );
                }
                context.SaveChanges();

                if (!context.Flights.Any())
                {
                    context.Flights.AddRange(
                        new Flight
                        {
                            airline = "Wizz Air",
                            planetype = "Airbus A320",
                            departure_time = DateTime.Now.AddDays(10),
                            arrival_time = DateTime.Now.AddDays(10).AddHours(2),
                            departure_city = "Budapest",
                            departure_country = "Hungary",
                            detination_city = "Vienna",
                            destination_country = "Austria",
                            departure_airport = "BUD",
                            destination_airport = "VIE",
                            duration = 120,
                            available_seats = 150,
                            price = 59
                        },
                        new Flight
                        {
                            airline = "Ryanair",
                            planetype = "Boeing 737",
                            departure_time = DateTime.Now.AddDays(15),
                            arrival_time = DateTime.Now.AddDays(15).AddHours(3),
                            departure_city = "Vienna",
                            departure_country = "Austria",
                            detination_city = "Barcelona",
                            destination_country = "Spain",
                            departure_airport = "BUD",
                            destination_airport = "BCN",
                            duration = 180,
                            available_seats = 200,
                            price = 79
                        },
                        new Flight
                        {
                            airline = "Lufthansa",
                            planetype = "Airbus A320",
                            departure_time = DateTime.Now.AddDays(5),
                            arrival_time = DateTime.Now.AddDays(5).AddHours(1.5),
                            departure_city = "Budapest",
                            departure_country = "Hungary",
                            detination_city = "Munich",
                            destination_country = "Germany",
                            departure_airport = "BUD",
                            destination_airport = "MUC",
                            duration = 90,
                            available_seats = 180,
                            price = 129
                        },
                        new Flight
                        {
                            airline = "British Airways",
                            planetype = "Boeing 777",
                            departure_time = DateTime.Now.AddDays(8),
                            arrival_time = DateTime.Now.AddDays(8).AddHours(2.5),
                            departure_city = "Vienna",
                            departure_country = "Austria",
                            detination_city = "London",
                            destination_country = "United Kingdom",
                            departure_airport = "BUD",
                            destination_airport = "LHR",
                            duration = 150,
                            available_seats = 220,
                            price = 159
                        }
                    );
                }
                context.SaveChanges();

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
                //            accommodation_id = 1,
                //            rating = 10,
                //            comment = "amazing place!",
                //            created_at = DateTime.Now
                //        },
                //        new Review
                //        {
                //            accommodation_id = 2,
                //            rating = 9,
                //            comment = "great location.",
                //            created_at = DateTime.Now
                //        }
                //    );
                //}

                context.SaveChanges();
            }
        }
    }
}
