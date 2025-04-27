using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using UtazasSzervezo_Library.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace UtazasSzervezo_UI.Pages.Bookings.Book
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
        private DateTime? _startDate;
        private DateTime? _endDate;

        public async Task<IActionResult> OnGetAsync(int accommodationId, DateTime? startDate, DateTime? endDate)
        {
            Booking = new Booking();
            _startDate = startDate;
            _endDate = endDate;

            var responseAccommodation = await _httpClient.GetAsync($"http://localhost:5133/api/Accommodation/{accommodationId}");

            if (!responseAccommodation.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await responseAccommodation.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Accommodation = JsonSerializer.Deserialize<Accommodation>(json, options);

            if (Accommodation == null)
            {
                return NotFound();
            }
            else
            {
                var nights = (Booking.end_date - Booking.start_date).Days;
                Booking.total_price = nights * Accommodation.price_per_night;
            }

            Booking.accommodation_id = accommodationId;

            if (startDate.HasValue && endDate.HasValue)
            {
                Booking.start_date = startDate.Value;
                Booking.end_date = endDate.Value;
            }
            else
            {
                Booking.start_date = DateTime.Today.AddDays(30);
                Booking.end_date = DateTime.Today.AddDays(33);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Booking.user_id = user.Id;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.Email;
                PhoneNumber = user.PhoneNumber;
                Country = user.Country;
                PostalCode = user.PostalCode.ToString();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int accommodationId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await OnGetAsync(accommodationId, _startDate, _endDate);
                    return Page();
                }

                if (Booking.start_date >= Booking.end_date)
                {
                    ModelState.AddModelError("", "Check-out date must be after check-in date.");
                    await OnGetAsync(accommodationId, _startDate, _endDate);
                    return Page();
                }

                // Ellenõrizzük a rendelkezésre állást az API-n keresztül
                var availabilityResponse = await _httpClient.GetAsync($"http://localhost:5133/api/Booking/CheckAvailability?accommodationId={accommodationId}&startDate={Booking.start_date:yyyy-MM-dd}&endDate={Booking.end_date:yyyy-MM-dd}");

                var availabilityJson = await availabilityResponse.Content.ReadAsStringAsync();
                var availabilityResult = JsonSerializer.Deserialize<bool>(availabilityJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (availabilityResult == false)
                {
                    ModelState.AddModelError("", "No rooms available for the selected dates.");
                    await OnGetAsync(accommodationId, _startDate, _endDate);
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
                    await OnGetAsync(accommodationId, _startDate, _endDate);
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

                var content = new StringContent(
                    JsonSerializer.Serialize(Booking, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("http://localhost:5133/api/Booking", content);

                if (response.IsSuccessStatusCode)
                {
                    BookingSuccessful = true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Failed to submit booking.{error}");
                }

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                await OnGetAsync(accommodationId, _startDate, _endDate);
                return Page();
            }
        }
    }

}