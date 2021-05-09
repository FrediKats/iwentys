using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess
{
    public class UnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        private Dictionary<(Type type, string name), object> _repositories = new Dictionary<(Type type, string Name), object>();

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (GenericRepository<TEntity>)GetOrAddRepository(typeof(TEntity), new GenericRepository<TEntity>(Context));
        }

        public async Task<int> CommitAsync()
        {
            //TODO: disable coz all test failed with exception
            //Context.EnsureAutoHistory();
            return await Context.SaveChangesAsync();
        }

        internal object GetOrAddRepository(Type type, object repo)
        {
            _repositories ??= new Dictionary<(Type type, string Name), object>();

            if (_repositories.TryGetValue((type, repo.GetType().FullName), out var repository)) return repository;
            _repositories.Add((type, repo.GetType().FullName), repo);
            return repo;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}