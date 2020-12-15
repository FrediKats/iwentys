using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Tools
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal readonly DbSet<TEntity> DbSet;
        internal readonly DbContext Context;

        public GenericRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAsync()
        {
            return DbSet;
        }

        public async Task<TEntity> GetByIdAsync<TKey>(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        //TODO: add return T
        public async Task InsertAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task DeleteAsync<TKey>(TKey id)
        {
            TEntity entityToDelete = await DbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
        }

        public async Task DeleteAsync(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}