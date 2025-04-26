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
            if (id == null)
            {
                return NotFound("Booking ID not provided.");
            }

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
                        ErrorMessage = "The requested booking is not a valid flight booking or data is missing.";
                        Booking = null;
                        return Page();
                    }

                    return Page();
                }
                else
                {
                    ErrorMessage = $"Error fetching booking details. Status code: {response.StatusCode}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCancelBookingAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Booking ID not provided for cancellation.");
            }

            try
            {
                var getResponse = await _httpClient.GetAsync($"http://localhost:5133/api/Booking/{id.Value}");

                if (!getResponse.IsSuccessStatusCode)
                {
                    if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return NotFound($"Booking with ID {id.Value} not found for cancellation.");
                    }
                    else
                    {
                        ErrorMessage = $"Error fetching booking details for cancellation. Status code: {getResponse.StatusCode}";
                        return RedirectToPage(new { id = id.Value });
                    }
                }

                var json = await getResponse.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var bookingToCancel = JsonSerializer.Deserialize<Booking>(json, options);

                if (bookingToCancel == null || bookingToCancel.Flight == null)
                {
                    ErrorMessage = "Could not retrieve flight booking details for cancellation.";
                    return RedirectToPage(new { id = id.Value });
                }

                if (bookingToCancel.Flight.departure_time <= DateTime.Now)
                {
                    ErrorMessage = "This flight booking cannot be cancelled as the departure time has already passed.";
                    return RedirectToPage(new { id = id.Value });
                }

                var deleteResponse = await _httpClient.DeleteAsync($"http://localhost:5133/api/Booking/{id.Value}");

                if (deleteResponse.IsSuccessStatusCode)
                {
                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = "<script>window.location.href='/Bookings/MyBookings';</script>"
                    };
                }
                else
                {
                    ErrorMessage = $"Failed to cancel the flight booking. Status code: {deleteResponse.StatusCode}";
                    return RedirectToPage(new { id = id.Value });
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred during flight booking cancellation: {ex.Message}";
                return RedirectToPage(new { id = id.Value });
            }
        }
    }
}
