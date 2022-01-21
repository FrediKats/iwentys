using System.Linq.Expressions;
using Iwentys.EntityManager.Common;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.DataAccess;

public static class DbContextExtensions
{
    public static async Task<TEntity> GetById<TEntity, TKey>(this DbSet<TEntity> repository, TKey key)
        where TEntity : class
    {
        return await repository.FindAsync(key) ?? throw EntityNotFoundException.Create(typeof(TEntity), key);
    }

    public static async Task<TEntity> GetSingle<TEntity>(this DbSet<TEntity> repository, Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        return await repository
            .Where(filter)
            .SingleOrDefaultAsync() ?? throw EntityNotFoundException.Create(typeof(TEntity));
    }
}