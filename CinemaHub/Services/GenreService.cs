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
    public class GenreService : TextSearchServiceBase<TextSearchRequest, Genre>, IGenreService
    {
        public GenreService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        protected override IQueryable<Genre> Query => dbContext.Genres.AsQueryable();
        protected override IQueryable<Genre> ExtractSearchQuery(TextSearchRequest request)
        {
            var query = Query;
            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(x => x.Name.ToLower().Contains(request.SearchText.ToLower()));
            }

            return query;
        }

        protected override async Task<Genre> BeforeAddAsync(Genre entity)
        {
            if (await Query.Where(x => entity.Name.ToLower() == x.Name.ToLower()).AnyAsync())
            {
                throw new ValidationException(new ValidationError("name", "Duplicate name"));
            }

            return entity;
        }

        protected override async Task<Genre> BeforeUpdateAsync(Guid id, Genre entity, Genre localEntity)
        {
            if (localEntity.Name != entity.Name)
            {
                if (await Query.Where(x => entity.Name.ToLower() == x.Name.ToLower()).AnyAsync())
                {
                    throw new ValidationException(new ValidationError("name", "Duplicate name"));
                }
            }

            return entity;
        }
    }
}
