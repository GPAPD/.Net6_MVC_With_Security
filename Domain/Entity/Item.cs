using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ItemName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        public long CatId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ImageUrl { get; set; }

        public Category Category { get; set; }
    }
}
