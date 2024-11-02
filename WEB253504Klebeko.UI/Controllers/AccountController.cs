using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WEB253504Klebeko.UI.Authorization;
using WEB253504Klebeko.UI.Models;

namespace WEB253504Klebeko.UI.Controllers
{
    public class AccountController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel user,
        [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest();
                }
                var result = await authService.RegisterUserAsync(user.Email,
                user.Password,
                user.Avatar);
                if (result.Result)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else return BadRequest(result.ErrorMessage);
            }
            return View(user);
        }

        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
            OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index",
            "Home")
            });
        }
        [HttpPost]
        public async Task Logout()
        {
            HttpContext.Session.Remove("Cart");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") });
        }
    }
}
