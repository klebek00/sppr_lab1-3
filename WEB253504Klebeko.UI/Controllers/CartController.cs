using Microsoft.AspNetCore.Mvc;

namespace WEB253504Klebeko.UI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
