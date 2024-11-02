using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB253504Klebeko.Domain.Entities;

namespace WEB253504Klebeko.Domain.Models
{
    public class CartItem
    {
        public CartItem(Medicines medicines, int amount = 1)
        {
            Medicines = medicines;
            Amount = amount;
        }

        public Medicines Medicines { get; set; }
        public int Amount { get; set; }
        public void addAmoint()
        {
            Amount += 1;
        }
        public double TotalPrice { get => Amount * Medicines.Price; }

    }
}
