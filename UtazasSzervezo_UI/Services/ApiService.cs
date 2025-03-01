using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public ApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ApiUrl"];
        }

        public async Task<List<Accommodation>> GetAccommodationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Accommodation>>($"{_apiUrl}accommodations");
        }
    }
}
