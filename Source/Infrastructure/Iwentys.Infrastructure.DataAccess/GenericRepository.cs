using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Infrastructure.DataAccess
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

        public IQueryable<TEntity> Get()
        {
            return DbSet;
        }

        public async Task<TEntity> FindByIdAsync<TKey>(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public TEntity Insert(TEntity entity)
        {
            EntityEntry<TEntity> result = DbSet.Add(entity);
            return result.Entity;
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
                DbSet.Attach(entityToDelete);

            DbSet.Remove(entityToDelete);
        }

        public void Delete(List<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                Delete(entity);
        }

        public void Update(List<TEntity> entitiesToUpdate)
        {
            entitiesToUpdate.ForEach(Update);
        }

        public void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}