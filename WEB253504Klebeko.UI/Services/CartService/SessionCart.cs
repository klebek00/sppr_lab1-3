using WEB253504Klebeko.Domain.Models;
using System.Text.Json;
using WEB253504Klebeko.Domain.Entities;
using System.Text.Json.Serialization;
using WEB253504Klebeko.UI.Extensions;

namespace WEB253504Klebeko.UI.Services.CartService
{
    public class SessionCart : Cart
    {
        [JsonIgnore]
        public ISession? Session { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
            SessionCart cart = session?.Get<SessionCart>("Cart")?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        public override void AddToCart(Medicines product)
        {
            base.AddToCart(product);
            Session?.Set("Cart", this);
        }
        public override void RemoveItems(int id)
        {
            base.RemoveItems(id);
            Session?.Set("Cart", this);
        }
        public override void ClearAll()
        {
            base.ClearAll();
            Session?.Remove("Cart");
        }
    }
}
