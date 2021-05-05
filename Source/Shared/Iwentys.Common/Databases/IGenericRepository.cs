using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Common.Databases
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> Get();
        Task<TEntity> FindByIdAsync<TKey>(TKey id);
        TEntity Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        void Delete(TEntity entityToDelete);
        void Delete(List<TEntity> entities);
        void Update(List<TEntity> entitiesToUpdate);
        void Update(TEntity entityToUpdate);
    }

    public static class GenericRepositoryExtensions
    {
        public static async Task<TEntity> GetById<TEntity, TKey>(this IGenericRepository<TEntity> repository, TKey key)
            where TEntity : class
        {
            return await repository.FindByIdAsync(key) ?? throw EntityNotFoundException.Create(typeof(TEntity), key);
        }

        public static async Task<TEntity> GetSingle<TEntity>(this IGenericRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter)
            where TEntity : class
        {
            return await repository
                .Get()
                .Where(filter)
                .SingleOrDefaultAsync() ?? throw EntityNotFoundException.Create(typeof(TEntity));
        }
    }
}