using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DemoDotnet.Models
{
    public class items
    {
        [Key]
        public string ItemID { get; set; }
        public string itemName { get; set; }

    }
}