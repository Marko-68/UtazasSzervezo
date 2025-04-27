using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using UtazasSzervezo_Library.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace UtazasSzervezo_UI.Pages.Bookings.Book
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

        private async Task<Flight?> GetFlightAsync(int flightId, bool addModelError = true)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5133/api/Flight/{flightId}");

            if (!response.IsSuccessStatusCode)
            {
                if (addModelError)
                {
                    ModelState.AddModelError("", "Could not fetch flight details.");
                }
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Flight>(json, options);
        }

        public async Task<IActionResult> OnGetAsync(int flightId)
        {
            Flight = await GetFlightAsync(flightId, false);
            if (Flight == null)
                return NotFound();

            Booking = new Booking
            {
                Flight = Flight,
                flight_id = flightId
            };

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
                Flight = await GetFlightAsync(flightId);
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var flight = await GetFlightAsync(flightId);
            if (flight == null)
            {
                return Page();
            }

            Booking.total_price = flight.price;
            Booking.status = "Pending";
            Booking.start_date = flight.departure_time; 
            Booking.end_date = flight.arrival_time;

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

            Flight = flight;

            return Page();
        }
    }
}