using Domain.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABC_Company.Models
{
    public class AdminItemModel
    {
        public long Id { get; set; }

        public string ItemName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }
    }
}
