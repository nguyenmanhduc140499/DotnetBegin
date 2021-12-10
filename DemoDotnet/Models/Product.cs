using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemoDotnet.Models
{
    public class Product
    {
        [Key]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string ProductID { get; set; }
        [StringLength(60, ErrorMessage = "something", MinimumLength = 3)]
        [Required]
        public string ProductName { get; set; }
        public string CategoryID { get; set; }
        // public string CategoryName { get; set; }
        public string ItemID { get; set; }
        [Range(1, 10000)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public string UnitPrice { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]
        public string Quantity { get; set; }
        public Category Category { get; set; }
    }
}