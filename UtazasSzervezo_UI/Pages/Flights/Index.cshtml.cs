using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Flights
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Flight> AllFlights { get; set; } = new();
        public List<Flight> FilteredFlights { get; set; } = new();
        public List<string> Airlines { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Airline { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinPrice { get; set; } = 0;
        public int? SliderMaxPrice { get; set; }

        private int? _maxPrice;
        [BindProperty(SupportsGet = true)]
        public int? MaxPrice
        {
            get => _maxPrice ?? SliderMaxPrice;
            set => _maxPrice = value;
        }

        [BindProperty(SupportsGet = true)]
        public string? DepartureCity { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DestinationCity { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DepartureDate { get; set; }

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync("http://localhost:5133/api/flight");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                AllFlights = JsonSerializer.Deserialize<List<Flight>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Flight>();

                Airlines = AllFlights.Select(f => f.airline).Distinct().OrderBy(a => a).ToList();

                if (AllFlights.Any())
                {
                    SliderMaxPrice = (int)AllFlights.Max(f => f.price);
                }

                FilteredFlights = ApplyFilters(AllFlights);
            }
        }

        private List<Flight> ApplyFilters(List<Flight> flights)
        {
            var filtered = flights.AsEnumerable();

            if (!string.IsNullOrEmpty(DepartureCity))
            {
                filtered = filtered.Where(f => 
                    (f.departure_city.Contains(DepartureCity, StringComparison.OrdinalIgnoreCase) ||
                    (f.departure_airport.Contains(DepartureCity, StringComparison.OrdinalIgnoreCase)) ||
                    (f.departure_country.Contains(DepartureCity, StringComparison.OrdinalIgnoreCase))
                    ));
            }

            if (!string.IsNullOrEmpty(DestinationCity))
            {
                filtered = filtered.Where(f => 
                    (f.detination_city.Contains(DestinationCity, StringComparison.OrdinalIgnoreCase) ||
                    (f.destination_airport.Contains(DestinationCity, StringComparison.OrdinalIgnoreCase)) ||
                    (f.destination_country.Contains(DestinationCity, StringComparison.OrdinalIgnoreCase))
                    ));
            }

            if (DepartureDate.HasValue)
            {
                filtered = filtered.Where(f => f.departure_time.Date == DepartureDate.Value.Date);
            }

            if (MinPrice.HasValue)
            {
                filtered = filtered.Where(f => f.price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                filtered = filtered.Where(f => f.price <= MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(Airline))
            {
                filtered = filtered.Where(f => f.airline == Airline);
            }

            return filtered.ToList();
        }
    }
}