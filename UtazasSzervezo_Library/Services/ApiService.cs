using Microsoft.Extensions.Configuration;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_Library.Services
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

    }
}
