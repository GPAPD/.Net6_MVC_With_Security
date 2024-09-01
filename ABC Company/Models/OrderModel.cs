using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC_Company.Models
{
    public class OrderModel
    {
        public long Id { get; set; }

        public long ShopcartId { get; set; }

        public ShopCart Shopcart { get; set; }

        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public string FristName { get; set; }

        public string SecondName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Contact { get; set; }

        public decimal ShipCost { get; set; }

        public decimal Tax { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public bool IsComplete { get; set; }

        public DateTime OrderDate { get; set; }

        public List<Order> MyOrderList { get; set; }

        public Order ThisOrder { get; set; }    
    }
}
