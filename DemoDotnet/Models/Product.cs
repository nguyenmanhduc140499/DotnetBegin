using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DemoDotnet.Models
{
    public class Product
    {
        [Key]
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string CategoryID { get; set; }
        public string ItemID { get; set; }
        public string UnitPrice { get; set; }
        public string Quantity { get; set; }
        public Category Category { get; set; }

    }
}