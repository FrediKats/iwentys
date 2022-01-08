using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iwentys.Common;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess;

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