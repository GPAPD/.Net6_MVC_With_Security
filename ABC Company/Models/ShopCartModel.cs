using Domain.Entity;

namespace ABC_Company.Models
{
    public class ShopCartModel
    {
        public ShopCart ShopCart { get; set; }

        public decimal NetTotal { get; set; }
        public decimal SubTotal { get; set;}
        public decimal Tax { get; set;}
        public decimal ShipCost { get; set;}
        public decimal Discounts { get; set;}
    }
}
