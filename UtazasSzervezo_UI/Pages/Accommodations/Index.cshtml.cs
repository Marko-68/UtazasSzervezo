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
        public int? MaxPersons { get; set; }

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
            if (MaxPersons.HasValue)
            {
                filtered = filtered.Where(a => a.max_person >= MaxPersons.Value);
            }

            if (!string.IsNullOrEmpty(AccommodationType))
            {
                filtered = filtered.Where(a => a.type == AccommodationType);
            }

            return filtered.ToList();
        }
    }
}