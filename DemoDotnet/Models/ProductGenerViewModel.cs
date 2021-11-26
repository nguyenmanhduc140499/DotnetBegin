using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DemoDotnet.Models
{
    public class ProductGenerViewModel
    {
        public List<Product> Products { get; set; }
        public SelectList Names { get; set; }
        public string ProductNames { get; set; }
        public string SearchString { get; set; }
    }
}