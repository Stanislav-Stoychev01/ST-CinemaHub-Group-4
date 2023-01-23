using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Data;
using CinemaHub.Domain;
using CinemaHub.Utility;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Services
{
    public abstract class TextSearchServiceBase<TSearchRequest, TEntity> : CrudServiceBase<TEntity>, ITextSearchService<TSearchRequest, TEntity>
        where TSearchRequest : TextSearchRequest
        where TEntity : Entity
    {
        protected TextSearchServiceBase(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        protected abstract IQueryable<TEntity> ExtractSearchQuery(TSearchRequest request);

        public virtual async Task<IEnumerable<TEntity>> SearchAsync(TSearchRequest request)
        {
            var query = ExtractSearchQuery(request);

            return await query.Sorted(request).Paged(request).ToListAsync();
        }

        public virtual async Task<int> CountAsync(TSearchRequest request)
        {
            return await ExtractSearchQuery(request).CountAsync();
        }
    }
}