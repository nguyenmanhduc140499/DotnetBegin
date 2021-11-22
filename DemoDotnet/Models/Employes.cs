using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DemoDotnet.Models
{
    public class Employes : Person
    {
        public string EmployesID { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}