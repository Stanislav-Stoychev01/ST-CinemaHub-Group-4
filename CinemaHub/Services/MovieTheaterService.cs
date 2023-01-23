using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Domain;

namespace CinemaHub.Services
{
    public class MovieTheaterService : TextSearchServiceBase<TextSearchRequest,MovieTheater>, IMovieTheaterService
    {
        public MovieTheaterService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<MovieTheater> Query => dbContext.MovieTheaters.AsQueryable();
        protected override IQueryable<MovieTheater> ExtractSearchQuery(TextSearchRequest request)
        {
            var query = Query;
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchText.ToLower()));
            }

            return query;
        }
    }
}
