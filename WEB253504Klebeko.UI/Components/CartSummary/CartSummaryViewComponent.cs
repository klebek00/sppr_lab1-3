using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.UI.Extensions;

namespace WEB253504Klebeko.UI.Components.CartSummary
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartSummaryViewComponent(Cart cart)
        {
            _cart = cart;
        }
        public IViewComponentResult Invoke() 
        {
            //var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();

            //return View(cart);

            return View(_cart);
        }
    }
}
