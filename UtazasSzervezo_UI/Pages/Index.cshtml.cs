using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
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

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Accommodations = JsonSerializer.Deserialize<List<AccommodationDto>>(json, options)
                                 ?? new List<AccommodationDto>();
            }
        }

        public IActionResult OnPostLaunchWpf()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
            {
                string exePath = @"C:\Users\gigle\Documents\suli\13\Vizsgaremek\UtazasSzervezo-master\UtazasSzervezo-master\UtazasSzervezo_Admin\bin\Debug\net8.0-windows\UtazasSzervezo_Admin.exe";

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = exePath,
                        UseShellExecute = true 
                    };
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Nem sikerült elindítani az admin alkalmazást: " + ex.Message);
                }
            }

            return RedirectToPage("/Index");
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