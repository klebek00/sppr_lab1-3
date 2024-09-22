using Microsoft.AspNetCore.Mvc;

namespace WEB253504Klebeko.UI.Components.CartSummary
{
    public class CartSummary : ViewComponent
    {
        public IViewComponentResult Invoke() { return View(); }
    }
}
