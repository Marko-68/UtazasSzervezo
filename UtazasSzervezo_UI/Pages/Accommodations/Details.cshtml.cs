using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using UtazasSzervezo_Library.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace UtazasSzervezo_UI.Pages.Accommodations
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<User> _userManager;
        public Accommodation? Accommodation { get; set; }
        public List<Review> Reviews { get; set; } = new();

        [BindProperty]
        [Required]
        [Range(1, 10)]
        public int Rating { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Comment is required")]
        public string Comment { get; set; } = string.Empty;

        public DetailsModel(HttpClient httpClient, UserManager<User> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadData(id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                await LoadData(id);
                return Page();
            }

            var userId = _userManager.GetUserId(User);
            var reviewToSubmit = new Review
            {
                rating = Rating,
                comment = Comment,
                accommodation_id = id,
                created_at = DateTime.Now,
                user_id = userId
            };

            var content = new StringContent(
                JsonSerializer.Serialize(reviewToSubmit),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5133/api/Review", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error submitting review. Please try again.");
                await LoadData(id);
                return Page();
            }

            return RedirectToPage(new { id });
        }

        private async Task LoadData(int id)
        {
            var accommodationResponse = await _httpClient.GetAsync($"http://localhost:5133/api/Accommodation/{id}");
            var reviewsResponse = await _httpClient.GetAsync($"http://localhost:5133/api/Review/ByAccommodation/{id}");

            if (accommodationResponse.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                var json = await accommodationResponse.Content.ReadAsStringAsync();
                Accommodation = JsonSerializer.Deserialize<Accommodation>(json, options);
            }

            if (reviewsResponse.IsSuccessStatusCode)
            {
                var reviewsJson = await reviewsResponse.Content.ReadAsStringAsync();
                Reviews = JsonSerializer.Deserialize<List<Review>>(reviewsJson) ?? new List<Review>();
            }
        }
    }
}