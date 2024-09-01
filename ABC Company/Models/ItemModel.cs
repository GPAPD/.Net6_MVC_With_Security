using System.ComponentModel.DataAnnotations;

namespace ABC_Company.Models
{
    public class ItemModel
    {
        public long Id { get; set; }

        public string ItemName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public long Category { get; set; }

        public string ImageUrl { get; set; }
    }
}
