using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Accommodations
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Accommodation> Accommodations { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString{ get; set; }

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
                var allAccommodations = JsonSerializer.Deserialize<List<Accommodation>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!string.IsNullOrEmpty(SearchString))
                {
                    Accommodations = allAccommodations?
                        .Where(a => a.name != null && a.name!.ToUpper().Contains(SearchString.ToUpper()) ||
                                    a.city != null && a.city!.ToUpper().Contains(SearchString.ToUpper()) ||
                                    a.country != null && a.country!.ToUpper().Contains(SearchString.ToUpper()))
                        .ToList() ?? new List<Accommodation>();
                }
                else
                {
                    Accommodations = allAccommodations ?? new List<Accommodation>();
                }

            }
        }
    }
}
