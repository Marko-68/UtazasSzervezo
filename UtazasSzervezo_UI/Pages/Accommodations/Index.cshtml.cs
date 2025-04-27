using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;
using System.Net.Http; // HttpClient miatt szükséges lehet, bár már be volt injektálva
using System.Threading.Tasks; // Task miatt szükséges
using System.Linq; // LINQ mûveletek miatt
using System.Collections.Generic; // List miatt
using System; // DateTime, StringComparison stb. miatt

namespace UtazasSzervezo_UI.Pages.Accommodations
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Accommodation> AllAccommodations { get; set; } = new();
        public List<Accommodation> FilteredAccommodations { get; set; } = new();
        public List<string> AccommodationTypes { get; set; } = new();
        public List<Amenity> Amenities { get; set; } = new(); // Maradunk az eredeti névnél

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
        [BindProperty(SupportsGet = true)]
        public List<int> SelectedAmenities { get; set; } = new();

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync()
        {
            var accommodationResponse = await _httpClient.GetAsync("http://localhost:5133/api/accommodation");
            if (accommodationResponse.IsSuccessStatusCode)
            {
                var json = await accommodationResponse.Content.ReadAsStringAsync();
                AllAccommodations = JsonSerializer.Deserialize<List<Accommodation>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Accommodation>();

                AccommodationTypes = AllAccommodations
                    .Select(a => a.type)
                    .Where(t => !string.IsNullOrEmpty(t)) 
                    .Distinct()
                    .OrderBy(t => t)
                    .ToList();
            }
            var amenitiesResponse = await _httpClient.GetAsync("http://localhost:5133/api/amenity");
            if (amenitiesResponse.IsSuccessStatusCode)
            {
                var amenitiesJson = await amenitiesResponse.Content.ReadAsStringAsync();
                Amenities = JsonSerializer.Deserialize<List<Amenity>>(amenitiesJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Amenity>();
            }
            FilteredAccommodations = await ApplyFilters(AllAccommodations);
        }

        private async Task<List<Accommodation>> ApplyFilters(List<Accommodation> accommodations)
        {
            var filtered = accommodations.AsEnumerable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                filtered = filtered.Where(a =>
                    (a.name != null && a.name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (a.city != null && a.city.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) ||
                    (a.country != null && a.country.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            if (Guests.HasValue && Guests.Value > 0) 
            {
                filtered = filtered.Where(a => a.guests >= Guests.Value);
            }

            if (!string.IsNullOrEmpty(AccommodationType))
            {
                filtered = filtered.Where(a => a.type == AccommodationType);
            }

            if (SelectedAmenities != null && SelectedAmenities.Any())
            {
                filtered = filtered.Where(a =>
                    a.AccommodationAmenities != null &&
                    SelectedAmenities.All(sa => a.AccommodationAmenities.Select(am => am.amenity_id).Contains(sa)));
            }

            if (CheckIn.HasValue && CheckOut.HasValue && CheckIn.Value < CheckOut.Value)
            {
                var accommodationsToCheck = filtered.ToList(); 
                var availableAccommodations = new List<Accommodation>(); 

                foreach (var a in accommodationsToCheck)
                {
                    var checkUrl = $"http://localhost:5133/api/booking/CheckAvailability?accommodationId={a.id}&startDate={CheckIn.Value:yyyy-MM-dd}&endDate={CheckOut.Value:yyyy-MM-dd}";
                    bool isAvailable = false;
                    try
                    {
                        var result = await _httpClient.GetAsync(checkUrl);
                        if (result.IsSuccessStatusCode)
                        {
                            var availableJson = await result.Content.ReadAsStringAsync();
                            isAvailable = JsonSerializer.Deserialize<bool>(availableJson);
                        }
                    }
                    catch (Exception ex)
                    {
                        isAvailable = false;
                    }

                    if (isAvailable)
                    {
                        availableAccommodations.Add(a);
                    }
                }
                filtered = availableAccommodations;
            }

            return filtered.ToList(); 
        }
    }
}