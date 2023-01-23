using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;

namespace CinemaHub.Services
{
    public interface ITextSearchService<TSearchRequest, TEntity> : ICrudService<TEntity> where TEntity : Entity
        where TSearchRequest: TextSearchRequest
    {
        Task<IEnumerable<TEntity>> SearchAsync(TSearchRequest request);
        Task<int> CountAsync(TSearchRequest request);
    }
}