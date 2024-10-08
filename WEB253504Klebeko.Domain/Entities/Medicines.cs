using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB253504Klebeko.Domain.Entities
{
    public class Medicines
    {
        public int Id { get; set; }
		public string Name { get; set; } //
        public string Description { get; set; } //
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; } //
        public string Image { get; set; } //
        public string Mime { get; set; }
    }
}
