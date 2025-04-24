using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using UtazasSzervezo_Library.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace UtazasSzervezo_UI.Pages.Bookings
{
    [Authorize]
    public class BookAccommodationModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<User> _userManager;

        public BookAccommodationModel(HttpClient httpClient, UserManager<User> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        [BindProperty]
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string LastName { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string Country { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string PostalCode { get; set; } = string.Empty;

        public Accommodation? Accommodation { get; set; }
        public bool BookingSuccessful { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(int accommodationId)
        {
            Booking = new Booking();
            var response = await _httpClient.GetAsync($"http://localhost:5133/api/Accommodation/{accommodationId}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Accommodation = JsonSerializer.Deserialize<Accommodation>(json, options);

            if (Accommodation == null)
            {
                return NotFound();
            }

            Booking.accommodation_id = accommodationId;
            Booking.start_date = DateTime.Today.AddDays(30);
            Booking.end_date = DateTime.Today.AddDays(33);
            Booking.total_price = (Booking.end_date - Booking.start_date).Days * Accommodation.price_per_night;
            Booking.status = "Pending";
            Booking.description = "Standard Book";

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Booking.user_id = user.Id;
                FirstName = user.FirstName ?? string.Empty;
                LastName = user.LastName ?? string.Empty;
                Email = user.Email ?? string.Empty;
                PhoneNumber = user.PhoneNumber ?? string.Empty;
                Country = user.Country ?? string.Empty;
                PostalCode = user.PostalCode.ToString() ?? string.Empty;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int accommodationId)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(accommodationId);
                return Page();
            }

            if (Booking.start_date >= Booking.end_date)
            {
                ModelState.AddModelError("", "Check-out date must be after check-in date.");
                await OnGetAsync(accommodationId);
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            Booking.user_id = user.Id;
            Booking.accommodation_id = accommodationId;

            var responseAccommodation = await _httpClient.GetAsync($"http://localhost:5133/api/Accommodation/{accommodationId}");
            if (!responseAccommodation.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Could not fetch accommodation details.");
                await OnGetAsync(accommodationId);
                return Page();
            }

            var json = await responseAccommodation.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var accommodation = JsonSerializer.Deserialize<Accommodation>(json, options);

            if (accommodation != null)
            {
                var nights = (Booking.end_date - Booking.start_date).Days;
                Booking.total_price = nights * accommodation.price_per_night;
            }

            Booking.status = "Pending";

            var content = new StringContent(
                JsonSerializer.Serialize(Booking, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5133/api/Booking", content);

            if (response.IsSuccessStatusCode)
            {
                BookingSuccessful = true;
                return Page();
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to submit booking. Server response: {error}");
            await OnGetAsync(accommodationId);
            return Page();
        }
    }
}