using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ShopCartItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("Item")]
        public long ItemId { get; set; }

        public Item Item { get; set; }

        [ForeignKey("ShopCart")]
        public long ShopCartId { get; set; }

        public ShopCart ShopCart { get; set; }

        public decimal ItemPrice { get; set; }

        public int Qty { get; set; }

    }
}
