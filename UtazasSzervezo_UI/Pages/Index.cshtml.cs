using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UtazasSzervezo_UI.Pages
{
    public class IndexModel : PageModel
    {
        public List<AccommodationDto> Accommodations { get; set; } = new();

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5133/");

            var response = await client.GetAsync("api/Accommodation");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Accommodations = JsonSerializer.Deserialize<List<AccommodationDto>>(json) ?? new List<AccommodationDto>();
            }
        }
    }

    public class AccommodationDto
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("price_per_night")]
        public decimal PricePerNight { get; set; }

        [JsonPropertyName("cover_img")]
        public string CoverImg { get; set; }
    }
}