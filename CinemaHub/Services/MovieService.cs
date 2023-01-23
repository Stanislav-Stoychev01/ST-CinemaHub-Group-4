using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Services
{
    public class MovieService : TextSearchServiceBase<MovieTextSearchRequest, Movie>, IMovieService
    {
        public MovieService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Movie> Query => dbContext.Movies.Include(x=>x.MovieGenres).ThenInclude(x=>x.Genre).AsQueryable();
        protected override IQueryable<Movie> ExtractSearchQuery(MovieTextSearchRequest request)
        {
            var query = Query;
            if (!string.IsNullOrEmpty(request.Genre))
            {
                query = query.Where(x => x.MovieGenres.Any(y => y.Genre.Name.ToLower().Contains(request.Genre.ToLower())));
            }
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchText.ToLower()));
            }

            if (!request.IncludeInactive.HasValue || !request.IncludeInactive.Value)
            {
                query = query.Where(x => x.IsActive);
            }

            return query;
        }

        public async Task<Movie> UpdateGenresAsync(Movie movie, List<Genre> genres)
        {
            foreach (var movieGenre in movie.MovieGenres.ToList())
            {
                if (genres.All(x => x.Id != movieGenre.Genre.Id))
                {
                    movie.MovieGenres.Remove(movieGenre);
                }
                else
                {
                    genres.Remove(movieGenre.Genre);
                }
            }

            foreach (var genre in genres)
            {
                movie.MovieGenres.Add(new MovieGenres() { Id = Guid.NewGuid(), Genre = genre, Movie = movie });
            }

          
            return await this.UpdateAsync(movie.Id, movie);
        }
    }
}
