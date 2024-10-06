using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;

namespace WEB253504Klebeko.UI.Services.MedicineService
{
    public class ApiMedicineService : IMedicineService
    {
        HttpClient _httpClient;

        JsonSerializerOptions _serializerOptions;
        private readonly HttpContext _httpContext;
        ILogger _logger;
        string _pageSize;
        public ApiMedicineService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiMedicineService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }
        public async Task<ResponseData<Medicines>> CreateMedicAsync(Medicines product, IFormFile? formFile)
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Medicines");
            var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data = await response
                    .Content
                    .ReadFromJsonAsync<ResponseData<Medicines>>(_serializerOptions);
                return data;
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
            return ResponseData<Medicines>.Error($"Объект не добавлен. Error:{response.StatusCode.ToString()}");
        }

        public async Task DeleteMedicAsync(int id)
        {
            var uri = new UriBuilder(_httpClient.BaseAddress!.AbsoluteUri, $"medicine/{id}").Uri;

            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not deleted. Error: {response.StatusCode}";
                _logger.LogError(errorMessage);
            }
        }


        //public async Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1)
        //{
        //    var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}medicines/");

        //    if (categoryNormalizedName != null)
        //    {
        //        urlString.Append($"{categoryNormalizedName}/");
        //    }
        //    if (pageNo > 1)
        //    {
        //        urlString.Append($"page{pageNo}");
        //    };
        //    // добавить размер страницы в строку запроса
        //    if (!_pageSize.Equals("3"))
        //    {
        //        urlString.Append(QueryString.Create("pageSize", _pageSize));
        //    }
        //    Console.WriteLine($"Requesting URL: {urlString}");
        //    // отправить запрос к API
        //    var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        //    if (response.IsSuccessStatusCode)
        //    {
        //        try
        //        {
        //            return await response
        //                    .Content
        //                    .ReadFromJsonAsync<ResponseData<ListModel<Medicines>>>(_serializerOptions);
        //        }
        //        catch (JsonException ex)
        //        {
        //            _logger.LogError($"-----> Ошибка: {ex.Message}");
        //            return ResponseData<ListModel<Medicines>>.Error($"Ошибка: {ex.Message}");
        //        }
        //    }
        //    _logger.LogError($"-----> Данные не получены от сервера. Error: { response.StatusCode.ToString()}");

        //    return ResponseData<ListModel<Medicines>>.Error($"1Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

        //}
        public async Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}medicines/");

            if (!string.IsNullOrEmpty(categoryNormalizedName))
            {
                urlString.Append($"category/{categoryNormalizedName}"); // используем category
            }

            if (pageNo > 1)
            {
                urlString.Append($"page{pageNo}");
            }

            if (!_pageSize.Equals("3"))
            {
                urlString.Append(QueryString.Create("pageSize", _pageSize));
            }

            Console.WriteLine($"Requesting URL: {urlString}");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Medicines>>>(_serializerOptions);
                }
                catch (JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<ListModel<Medicines>>.Error($"Ошибка: {ex.Message}");
                }
            }

            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode}");
            return ResponseData<ListModel<Medicines>>.Error($"Данные не получены от сервера. Error: {response.StatusCode}");
        }



        public async Task UpdateMedicAsync(int id, Medicines product, IFormFile? formFile)
        {
            var uri = new UriBuilder(_httpClient.BaseAddress!.ToString(), "medicines").Uri;

            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not updated. Error {response.StatusCode}";
                _logger.LogError(errorMessage);
            }

        }
        public async Task<ResponseData<Medicines>> GetMedicByIdAsync(int id)
        {
            var uri = new UriBuilder(_httpClient.BaseAddress!.ToString(), "medicines").Uri;

            var response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not recieved. Error {response.StatusCode}";
                _logger.LogError(errorMessage);

                return ResponseData<Medicines>.Error("Medicines not found");

            }

            var data = await response.Content.ReadFromJsonAsync<ResponseData<Medicines>>(_serializerOptions);
            return data!;
        }
    }
}
