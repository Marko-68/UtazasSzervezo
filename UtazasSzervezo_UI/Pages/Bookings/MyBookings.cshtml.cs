using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Bookings
{
    [Authorize]
    public class MyBookingsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Booking> Bookings { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public bool ShowFlights { get; set; } = false;

        public MyBookingsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5133/api/Booking");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var allBookings = JsonSerializer.Deserialize<List<Booking>>(json, options);

                if (allBookings != null)
                {
                    //Szûrés a kiválasztott típus szerint
                    Bookings = allBookings.Where(b => ShowFlights ? b.flight_id != null : b.accommodation_id != null).ToList();
                }
            }
        }
    }
}