using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options)
        {           
        }
        public ApplicationDBContext()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //DataSeeding
            modelBuilder.Entity<Genre>()
                        .HasData(new Genre { Id = 1, Name = "Action" },
                        new Genre { Id = 2, Name = "Drama" },
                        new Genre { Id = 3, Name = "Crime" },
                        new Genre { Id = 4, Name = "Biography" },
                        new Genre { Id = 5, Name = "Adventure" },
                        new Genre { Id = 6, Name = "Horror" },
                        new Genre { Id = 7, Name = "Romance" },
                        new Genre { Id = 8, Name = "Comedy" },
                        new Genre { Id = 9, Name = "War" },
                        new Genre { Id = 10, Name = "Fantasy" },
                        new Genre { Id = 11, Name = "Historical" },
                        new Genre { Id = 12, Name = "Science fiction" },
                        new Genre { Id = 13, Name = "Thriller" },
                        new Genre { Id = 14, Name = "Western" } );
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
