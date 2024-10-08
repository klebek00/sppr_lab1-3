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

        public async Task<ResponseData<Category>> GetCategoryByIdAsync(int categoryId)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Categories/{categoryId}");
            _logger.LogInformation("Requesting URL: {url}", urlString.ToString());

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not received. Error {response.StatusCode}";
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("{ErrorMessage}. Response content: {Content}", errorMessage, responseContent);

                return ResponseData<Category>.Error("Category not found");
            }

            var categoryResponse = await response.Content.ReadFromJsonAsync<ResponseData<Category>>(_serializerOptions);
            if (categoryResponse == null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Deserialization returned null for response: {ResponseContent}", responseContent);
                return ResponseData<Category>.Error("Failed to deserialize category.");
            }

            return categoryResponse;
        }

    }
}
