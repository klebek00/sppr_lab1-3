using Microsoft.AspNetCore.Http;
using WEB253504Klebeko.UI.Services.Authentication;

namespace WEB253504Klebeko.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly HttpContext _httpContext;
        private readonly ITokenAccessor _tokenAccessor;

        public ApiFileService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ITokenAccessor tokenAccessor)
        {
            _httpClient = httpClient;
            _httpContext = httpContextAccessor.HttpContext;
            _tokenAccessor = tokenAccessor;
        }
        public async Task DeleteFileAsync(string file)
        {
            // Создайте URL для DELETE-запроса
            var fileName = Path.GetFileName(file);
            var requestUri = $"https://localhost:7002/api/files/{fileName}";
            Console.WriteLine($"Request URI: {requestUri}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri)
            };

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            // Отправить запрос на удаление файла
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Ошибка удаления файла: {response.StatusCode}");
            }
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            // Создать объект запроса
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };
            // Сформировать случайное имя файла, сохранив расширение
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            // Создать контент, содержащий поток загруженного файла
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);
            // Поместить контент в запрос
            request.Content = content;
            // Отправить запрос к API

            _tokenAccessor.SetAuthorizationHeaderAsync(_httpClient);
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Вернуть полученный Url сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }
    }
}
