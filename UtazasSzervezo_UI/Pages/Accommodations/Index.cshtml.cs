using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Pages.Accommodations
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public List<Accommodation> Accommodations { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; } 

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

                Accommodations = Search(allAccommodations, SearchString);
            }
            else
            {
                Accommodations = new List<Accommodation>();
            }
        }

        private List<Accommodation> Search(List<Accommodation>? accommodations, string? searchString)
        {
            if (accommodations == null) return new List<Accommodation>();

            if (!string.IsNullOrEmpty(searchString))
            {
                return accommodations
                    .Where(a => (a.name != null && a.name!.ToUpper().Contains(searchString.ToUpper())) ||
                                (a.city != null && a.city!.ToUpper().Contains(searchString.ToUpper())) ||
                                (a.country != null && a.country!.ToUpper().Contains(searchString.ToUpper())))
                    .ToList();
            }

            return accommodations;
        }
    }
}
