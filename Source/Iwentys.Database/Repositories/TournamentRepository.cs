using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class TournamentRepository : IGenericRepository<TournamentEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public TournamentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TournamentEntity> Read()
        {
            return _dbContext.Tournaments;
        }

        public TournamentEntity ReadById(int key)
        {
            return _dbContext.Tournaments.Find(key);
        }

        public TournamentEntity Update(TournamentEntity entity)
        {
            EntityEntry<TournamentEntity> createdEntity = _dbContext.Tournaments.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.Tournaments.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public TournamentEntity Create(TournamentEntity entity)
        {
            EntityEntry<TournamentEntity> createdEntity = _dbContext.Tournaments.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }
    }
}