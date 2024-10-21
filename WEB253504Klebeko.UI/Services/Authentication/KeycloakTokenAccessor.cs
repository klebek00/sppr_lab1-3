using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using NuGet.Common;
using System;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using WEB253504Klebeko.UI.HelperClasses;

namespace WEB253504Klebeko.UI.Services.Authentication
{
    public class KeycloakTokenAccessor : ITokenAccessor
    {
        private readonly KeycloakData _keycloakData;
        private readonly HttpContext? _httpContext;
        private readonly HttpClient _httpClient;
        public KeycloakTokenAccessor(IOptions<KeycloakData> options, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _keycloakData = options.Value;
            _httpContext = httpContextAccessor.HttpContext;
            _httpClient = httpClient;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            // Если пользователь вошел в систему, получить его токен
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                return await _httpContext.GetTokenAsync("access_token");
            }
            // Если пользователь не входил в систему, получить токен клиента
            // Keycloak token endpoint
            var requestUri =
            $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/token";
        // Http request content
            HttpContent content = new FormUrlEncodedContent([
                new KeyValuePair<string,string>("client_id",_keycloakData.ClientId),
                new KeyValuePair<string,string>("grant_type","client_credentials"),
                new
                KeyValuePair<string,string>("client_secret",_keycloakData.ClientSecret)
            ]);

            var response = await _httpClient.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode.ToString());
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Content: {jsonString}");
            return JsonObject.Parse(jsonString)["access_token"].GetValue<string>();
        }
        public async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            string token = await GetAccessTokenAsync();
            Console.WriteLine($"JWT Token: {token}");

            httpClient
            .DefaultRequestHeaders
            .Authorization = new AuthenticationHeaderValue("bearer", token);
        }
    }
}
