using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoDotnet.Models;
using DemoDotnet.Data;

namespace DemoDotnet.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<DemoDotnet.Models.Student> Student { get; set; }
        public DbSet<DemoDotnet.Models.Person> Person { get; set; }
        public DbSet<DemoDotnet.Models.Product> Product { get; set; }
        public DbSet<DemoDotnet.Models.Category> Category { get; set; }
        public DbSet<DemoDotnet.Models.Employes> Employes { get; set; }
        public DbSet<DemoDotnet.Models.items> items { get; set; }
        public DbSet<DemoDotnet.Models.Movie> Movie { get; set; }

    }
}
