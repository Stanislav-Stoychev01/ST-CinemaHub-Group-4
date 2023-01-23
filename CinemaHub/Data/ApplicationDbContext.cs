using System;
using CinemaHub.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 

            builder.Entity<MovieGenres>()
                .HasKey(bc => new { bc.GenreId, bc.MovieId });
            builder.Entity<MovieGenres>()
                .HasOne(bc => bc.Genre)
                .WithMany(b => b.MovieGenres)
                .HasForeignKey(bc => bc.GenreId);
            builder.Entity<MovieGenres>()
                .HasOne(bc => bc.Movie)
                .WithMany(b => b.MovieGenres)
                .HasForeignKey(bc => bc.MovieId);
        }

        public DbSet<MovieTheater> MovieTheaters { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieScreening> MovieScreenings { get; set; }
        public DbSet<TicketPrice> TicketPrices { get; set; }


    }
}
