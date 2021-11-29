using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using DemoDotnet.Models;
using DemoDotnet.Data;

namespace DemoDotnet.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDBContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDBContext>>()))
            {
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-1-11"),
                        Genre = "Romantic Comedy",
                        Rating = "R",
                        Price = 7.99M
                    }

                    // new Student
                    // {
                    //     StudentID = "ST02",
                    //     //StudentName = "Nguyen Manh Duc 2",
                    //     Address = "Ha Noi",
                    // },

                    // new Student
                    // {
                    //     StudentID = "ST03",
                    //     // StudentName = "Nguyen Manh Duc 3",
                    //     Address = "Ha Noi",
                    // }
                );
                context.SaveChanges();
            }
        }
    }
}