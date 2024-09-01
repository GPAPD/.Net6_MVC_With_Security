using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Shopcart")]
        public long ShopcartId { get; set; }

        public ShopCart Shopcart { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string FristName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string SecondName { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string Address1 { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string Address2 { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string City { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string PostalCode { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Country { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Contact { get; set; }

        public decimal ShipCost { get; set; }

        public decimal Tax { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }

        public bool IsComplete { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
