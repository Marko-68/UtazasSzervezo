using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Admin.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            return await _httpClient.GetFromJsonAsync<T>(uri);
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T data)
        {
            return await _httpClient.PostAsJsonAsync(uri, data);
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T data)
        {
            return await _httpClient.PutAsJsonAsync(uri, data);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return await _httpClient.DeleteAsync(uri);
        }
    }
}
