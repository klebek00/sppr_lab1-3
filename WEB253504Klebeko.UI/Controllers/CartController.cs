using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB253504Klebeko.UI.Services.MedicineService;
using WEB253504Klebeko.Domain.Models;
using WEB253504Klebeko.UI.Extensions;

namespace WEB253504Klebeko.UI.Controllers
{
    public class CartController : Controller
    {
        private readonly IMedicineService _medicineService;
        private readonly Cart _cart;

        public CartController(IMedicineService medicineService, Cart cart)
        {
            _medicineService = medicineService;
            _cart = cart;
        }
        public IActionResult Index()
        {
            //var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
            return View(_cart);

        }
        [Authorize]
        [Route("[controller]/add/{id:int}")]
        public async Task<ActionResult> Add(int id, string returnUrl)
        {
            //Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            //var data = await _medicineService.GetMedicByIdAsync(id);
            //if (data.Successfull)
            //{
            //    cart.AddToCart(data.Data);
            //    HttpContext.Session.Set<Cart>("cart", cart);
            //}
            //return Redirect(returnUrl);

            var data = await _medicineService.GetMedicByIdAsync(id);
            if (data.Successfull)
            {
                _cart.AddToCart(data.Data);
            }
            return Redirect(returnUrl);
        }

        [Authorize]
        [Route("[controller]/remove/{id:int}")]
        public async Task<ActionResult> Remove(int id, string returnUrl)
        {
            //Cart cart = HttpContext.Session.Get<Cart>("cart") ?? new();
            //var data = await _medicineService.GetMedicByIdAsync(id);
            //if (data.Successfull)
            //{
            //    cart.RemoveItems(data.Data.Id);
            //    HttpContext.Session.Set<Cart>("cart", cart);
            //}
            //return Redirect(returnUrl);

            var data = await _medicineService.GetMedicByIdAsync(id);
            if (data.Successfull)
            {
                _cart.RemoveItems(data.Data.Id);
            }
            return Redirect(returnUrl);
        }

    }
}
