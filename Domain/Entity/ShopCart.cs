using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ShopCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

		[ForeignKey("ApplicationUser")]
		public string ApplicationUserId { get; set; }

		public ApplicationUser ApplicationUser { get; set; }
        
        public decimal? Tax { get; set; }

        public decimal? ShipCost { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime UpdatedDate { get; set; }

        public ICollection<ShopCartItems> ShopCartItems { get; set; }


    }
}
