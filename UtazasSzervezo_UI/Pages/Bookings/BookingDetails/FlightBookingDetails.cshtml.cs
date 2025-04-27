using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Bookings.BookingDetails
{
    [Authorize]
    public class FlightBookingDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public Booking? Booking { get; set; }
        public string? ErrorMessage { get; set; }

        public FlightBookingDetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:5133/api/Booking/{id.Value}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Booking = JsonSerializer.Deserialize<Booking>(json, options);

                    if (Booking == null || Booking.flight_id == null || Booking.Flight == null)
                    {
                        ErrorMessage = "The requested booking is not valid";
                        Booking = null;
                        return Page();
                    }

                    return Page();
                }
                else
                {
                    ErrorMessage = $"Error:{response.StatusCode}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCancelBookingAsync(int? id)
        {
            try
            {
                var getResponse = await _httpClient.GetAsync($"http://localhost:5133/api/Booking/{id.Value}");

                if (!getResponse.IsSuccessStatusCode)
                {
                    ErrorMessage = $"Error: {getResponse.StatusCode}";
                    return NotFound($"Booking not found");
                }

                var json = await getResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var bookingToCancel = JsonSerializer.Deserialize<Booking>(json, options);

                if (bookingToCancel == null || bookingToCancel.Flight == null)
                {
                    ErrorMessage = "Could not retrieve flight booking details for cancellation";
                    return RedirectToPage(new { id = id.Value });
                }

                if (bookingToCancel.Flight.departure_time <= DateTime.Now)
                {
                    ErrorMessage = "This flight booking cannot be cancelled because the departure time has already passed.";
                    return RedirectToPage(new { id = id.Value });
                }

                var deleteResponse = await _httpClient.DeleteAsync($"http://localhost:5133/api/Booking/{id.Value}");

                if (deleteResponse.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Bookings/MyBookings");
                }
                else
                {
                    ErrorMessage = $"Failed to cancel the flight booking: {deleteResponse.StatusCode}";
                    return RedirectToPage(new { id = id.Value });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unexpected error:{ex.Message}";
                return RedirectToPage(new { id = id.Value });
            }
        }
    }
}
