namespace WEB253504Klebeko.UI.Services.Authentication
{
    public interface ITokenAccessor
    {
        Task<string> GetAccessTokenAsync();
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }
}
