using NuGet.Protocol;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;

namespace WEB253504Klebeko.UI.Services.CategoryService
{
    public class ApiCategoryService : ICategoryService
    {
        HttpClient _httpClient;
        ILogger _logger;
        JsonSerializerOptions _serializerOptions;
        IConfiguration _configuration;

        public ApiCategoryService(IConfiguration configuration, HttpClient httpClient, ILogger<ApiCategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Categories");
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not recieved. Error {response.StatusCode}";
                _logger.LogError(errorMessage);

                return ResponseData<List<Category>>.Error("Category not found");
            }

            var beerList = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>(_serializerOptions);
            return beerList!;
        }
    }
}
