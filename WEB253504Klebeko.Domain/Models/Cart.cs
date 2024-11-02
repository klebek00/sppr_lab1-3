using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB253504Klebeko.Domain.Entities;

namespace WEB253504Klebeko.Domain.Models
{
    public class Cart
    {
        public Dictionary<int, CartItem> CartItems { get; set; } = new();
        public virtual void AddToCart(Medicines medicines)
        {
            if (CartItems.ContainsKey(medicines.Id))
            {
                CartItems[medicines.Id].addAmoint();
            }
            else
            {
                CartItems.Add(medicines.Id, new CartItem(medicines));
            }
        }
        public virtual void RemoveItems(int id)
        {
            CartItems.Remove(id);
        }
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }
        public int Count { get => CartItems.Sum(item => item.Value.Amount); }
        public double TotalPrice
        {
            get => CartItems.Sum(item => item.Value.TotalPrice);
        }
    }
}

