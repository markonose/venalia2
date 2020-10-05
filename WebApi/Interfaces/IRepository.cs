using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Enums;

namespace WebApi.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task AddAsync(TEntity entity);
        Task DeleteAsync(Guid id);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<List<TEntity>> ListAsync(string query, string fieldName, OrderByDirection direction, long offset, int limit);
        Task UpdateAsync(TEntity entity);
    }
}
