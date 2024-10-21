using System.Net.Http;
using System.Text;
using System.Text.Json;
using WEB253504Klebeko.Domain.Entities;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.UI.Services.FileService;
using WEB253504Klebeko.UI.Services.Authentication;

namespace WEB253504Klebeko.UI.Services.MedicineService
{
    public class ApiMedicineService : IMedicineService
    {
        HttpClient _httpClient;

        JsonSerializerOptions _serializerOptions;
        private readonly HttpContext _httpContext;
        ILogger _logger;
        string _pageSize;
        private readonly IFileService _fileService;

        ITokenAccessor _tokenAccessor;

        public ApiMedicineService(HttpClient httpClient, IConfiguration configuration, ILogger<ApiMedicineService> logger, IFileService fileService, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _fileService = fileService;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _tokenAccessor = tokenAccessor;
        }
        public async Task<ResponseData<Medicines>> CreateMedicAsync(Medicines product, IFormFile? formFile)
        {
            product.Image = "Images/noimage.jpg";

            // Сохранить файл изображения
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                    product.Image = imageUrl;
            }

            _logger.LogInformation($"Создание медикамента: {product.Name}, CategoryId: {product.CategoryId}");

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Medicines");
            _logger.LogInformation($"Отправка запроса на: {uri}");

            try
            {
                _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

                var response = await _httpClient.PostAsJsonAsync(uri, product, _serializerOptions);

                // Логируем полученный статус и контент
                _logger.LogInformation($"Получен статус: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadFromJsonAsync<ResponseData<Medicines>>(_serializerOptions);
                    return data;
                }

                // Если статус не успешный, логируем содержимое ошибки
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"-----> объект не создан. Ошибка: {response.StatusCode} - Контент: {errorContent}");

                return ResponseData<Medicines>.Error($"Объект не добавлен. Ошибка: {response.StatusCode}, детали: {errorContent}");
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Ошибка при отправке HTTP-запроса");
                return ResponseData<Medicines>.Error("Ошибка сети или недоступен сервер.");
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Ошибка сериализации/десериализации JSON");
                return ResponseData<Medicines>.Error("Ошибка обработки данных.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Общая ошибка при создании медикамента");
                return ResponseData<Medicines>.Error("Объект не добавлен из-за неизвестной ошибки.");
            }
        }



        public async Task DeleteMedicAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}medicines/{id}");

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.DeleteAsync(uri);
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not deleted. Error: {response.StatusCode}";
                _logger.LogError(errorMessage);
            }
        }
        public async Task<ResponseData<ListModel<Medicines>>> GetMedicListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = -1)
        {
            pageSize = pageSize == -1 ? 3 : pageSize;

            _pageSize = pageSize.ToString();

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

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

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
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                // Добавить в объект Url изображения
                if (!string.IsNullOrEmpty(imageUrl))
                    product.Image = imageUrl;
            }
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}medicines/{id}");

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.PutAsJsonAsync(uri, product, _serializerOptions);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not updated. Error {response.StatusCode}";
                _logger.LogError(errorMessage);
            }

        }
        public async Task<ResponseData<Medicines>> GetMedicByIdAsync(int id)
        {
            var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}medicines/{id}");

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);

            var response = await _httpClient.GetAsync(uri);

            var content = await response.Content.ReadAsStringAsync();
            _logger.LogError($"-----> object not created. Error: {response.StatusCode}, Content: {content}");


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
