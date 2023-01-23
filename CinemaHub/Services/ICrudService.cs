using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CinemaHub.Controllers.Model;
using CinemaHub.Domain;
using Microsoft.EntityFrameworkCore;

namespace CinemaHub.Services
{
    public interface ICrudService<TEntity> where TEntity : Entity
    {
        Task<IEnumerable<TEntity>> FindAllAsync(PaginatedRequest request);
        Task<int> CountAllAsync();
        Task<TEntity> GetByIdAsync(Guid id, bool hasTracking = true);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(Guid id, TEntity entity);
        Task DeleteAsync(Guid id);
        void DetachLocal<T>(Entity entity, EntityState newState) where T : Entity;


    }
}