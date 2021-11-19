using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DemoDotnet.Models
{
    public class Student : Person
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string Address { get; set; }

    }
}