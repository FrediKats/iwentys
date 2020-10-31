using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Microsoft.EntityFrameworkCore;
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

        public Task<TournamentEntity> ReadById(int key)
        {
            return _dbContext.Tournaments.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<TournamentEntity> Update(TournamentEntity entity)
        {
            EntityEntry<TournamentEntity> createdEntity = _dbContext.Tournaments.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> Delete(int key)
        {
            _dbContext.Tournaments.Remove(this.Get(key));
            return _dbContext.SaveChangesAsync();
        }

        public TournamentEntity Create(TournamentEntity entity)
        {
            EntityEntry<TournamentEntity> createdEntity = _dbContext.Tournaments.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }
    }
}