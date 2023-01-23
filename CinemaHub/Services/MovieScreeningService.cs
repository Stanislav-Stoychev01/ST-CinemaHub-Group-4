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
    public class MovieScreeningService : TextSearchServiceBase<MovieScreeningTextSearchRequest, MovieScreening>, IMovieScreeningService
    {
        public MovieScreeningService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<MovieScreening> Query => dbContext.MovieScreenings.Include(x=>x.Movie).Include(x=>x.Theater).AsQueryable();
        protected override IQueryable<MovieScreening> ExtractSearchQuery(MovieScreeningTextSearchRequest request)
        {
            var query = Query;
            if (request.MovieId.HasValue)
            {
                query = query.Where(x => x.Movie.Id == request.MovieId.Value);
            }

            if (request.TheaterId.HasValue)
            {
                query = query.Where(x => x.Theater.Id == request.TheaterId.Value);
            }

            if (request.FromDateTime.HasValue)
            {
                query = query.Where(x => x.DateTime > request.FromDateTime.Value);
            }

            if (request.ToDateTime.HasValue)
            {
                query = query.Where(x => x.DateTime < request.ToDateTime.Value);
            }

            if (request.IsPremiere.HasValue)
            {
                query = query.Where(x => x.IsPremiere == request.IsPremiere.Value);
            }

            return query;
        }
    }
}
