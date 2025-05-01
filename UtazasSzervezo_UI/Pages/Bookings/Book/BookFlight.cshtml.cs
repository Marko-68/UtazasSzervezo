using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using UtazasSzervezo_Library.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace UtazasSzervezo_UI.Pages.Bookings
{
    [Authorize]
    public class BookFlightModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<User> _userManager;

        public BookFlightModel(HttpClient httpClient, UserManager<User> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        [BindProperty]
        public Booking Booking { get; set; } = new Booking();

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

        public Flight? Flight { get; set; }

        public bool BookingSuccessful { get; set; } = false;


        public async Task<IActionResult> OnGetAsync(int flightId)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5133/api/Flight/{flightId}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Flight = JsonSerializer.Deserialize<Flight>(json, options);

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

        public async Task<IActionResult> OnPostAsync(int flightId)
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(flightId);
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            Booking.user_id = userId;
            Booking.flight_id = flightId;
            Booking.accommodation_id = null;
            Booking.Accommodation = null;

            var flightResponse = await _httpClient.GetAsync($"http://localhost:5133/api/Flight/{flightId}");
            if (flightResponse.IsSuccessStatusCode)
            {
                var json = await flightResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Flight = JsonSerializer.Deserialize<Flight>(json, options);
            }
            else
            {
                ModelState.AddModelError("", "Error getting flight data");
                return Page();
            }

            Booking.total_price = Flight.price;
            Booking.start_date = Flight.departure_time;
            Booking.end_date = Flight.arrival_time;

            var content = new StringContent(
                JsonSerializer.Serialize(Booking, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5133/api/Booking", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error: {error}");
                await OnGetAsync(flightId);
                return Page();
            }

            BookingSuccessful = true;
            return Page();
        }

    }
}