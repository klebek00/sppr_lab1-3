namespace WEB253504Klebeko.UI.Authorization
{
    public interface IAuthService
    {
        Task<(bool Result, string ErrorMessage)> RegisterUserAsync(string email, string password, IFormFile? avatar);
    }
}
