using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Flights
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public Flight? Flight  { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Airline { get; set; }
        public DetailsModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadData(id);
            return Page();
        }
        private async Task LoadData(int id)
        {
            var response = await _httpClient.GetAsync($"http://localhost:5133/api/flight/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Flight = JsonSerializer.Deserialize<Flight>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new Flight();

            }
        }
    }
}
