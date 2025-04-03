using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Bookings
{
    public class MyBookingsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Booking> Bookings { get; set; } = new();

        public MyBookingsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5133/api/bookings");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var allAccommodations = JsonSerializer.Deserialize<List<Booking>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            }
            else
            {
                Bookings = new List<Booking>();
            }
        }
    }
}
