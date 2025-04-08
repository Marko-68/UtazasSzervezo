using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Accommodations
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Accommodation> AllAccommodations { get; set; } = new();
        public List<Accommodation> FilteredAccommodations { get; set; } = new();
        public List<string> AccommodationTypes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? CheckIn { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? CheckOut { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Guests { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? AccommodationType { get; set; }

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5133/api/accommodation");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                AllAccommodations = JsonSerializer.Deserialize<List<Accommodation>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Accommodation>();

                AccommodationTypes = AllAccommodations
                    .Select(a => a.type)
                    .Distinct()
                    .OrderBy(t => t)
                    .ToList();

                FilteredAccommodations = ApplyFilters(AllAccommodations);
            }
        }

        private List<Accommodation> ApplyFilters(List<Accommodation> accommodations)
        {
            var filtered = accommodations.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                filtered = filtered.Where(a =>
                    (a.name != null && a.name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (a.city != null && a.city.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (a.country != null && a.country.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }
            if (Guests.HasValue)
            {
                filtered = filtered.Where(a => a.guests >= Guests.Value);
            }

            if (!string.IsNullOrEmpty(AccommodationType))
            {
                filtered = filtered.Where(a => a.type == AccommodationType);
            }

            //akkor jelenítjük meg, ha legalább 1 elérhetõ szoba van
            if (CheckIn.HasValue && CheckOut.HasValue && CheckIn.Value < CheckOut.Value)
            {
                
                filtered = filtered.Where(a => a.available_rooms > 0);
            }


            return filtered.ToList();
        }
    }
}