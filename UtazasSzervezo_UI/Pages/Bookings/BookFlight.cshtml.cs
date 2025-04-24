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
            Booking = new Booking();
            var response = await _httpClient.GetAsync($"http://localhost:5133/api/Flight/{flightId}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Flight = JsonSerializer.Deserialize<Flight>(json, options);

            Booking.flight_id = flightId;
            Booking.start_date = DateTime.Today.AddDays(30);
            Booking.end_date = DateTime.Today.AddDays(33);
            Booking.total_price = Flight.price * 3;
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

        public async Task<IActionResult> OnPostAsync(int flightId)
        {
            Console.WriteLine("Booking info:");
            Console.WriteLine(JsonSerializer.Serialize(Booking, new JsonSerializerOptions { WriteIndented = true }));

            if (!ModelState.IsValid)
            {
                await OnGetAsync(flightId);
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            Booking.user_id = userId;
            Booking.flight_id = flightId;

            //Flight objektum újratöltése
            var responseFlight = await _httpClient.GetAsync($"http://localhost:5133/api/Flight/{flightId}");
            if (responseFlight.IsSuccessStatusCode)
            {
                var json = await responseFlight.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Booking.Flight = JsonSerializer.Deserialize<Flight>(json, options);
            }

            var content = new StringContent(
                JsonSerializer.Serialize(Booking),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5133/api/Booking", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to submit booking.");
                await OnGetAsync(flightId);
                return Page();
            }

            if (response.IsSuccessStatusCode)
            {
                BookingSuccessful = true;
                return Page();
            }
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", $"Failed to submit booking. Server response: {error}");
            await OnGetAsync(flightId);
            return Page();
        }
    }
}
