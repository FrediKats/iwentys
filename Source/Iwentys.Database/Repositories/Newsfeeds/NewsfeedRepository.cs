using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.Newsfeeds.Repositories;
using Iwentys.Models.Entities.Newsfeeds;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Repositories.Newsfeeds
{
    public class NewsfeedRepository : INewsfeedRepository
    {
        private readonly IwentysDbContext _dbContext;

        public NewsfeedRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<SubjectNewsfeedEntity>> GetSubjectNewsfeeds(int subjectId)
        {
            return _dbContext
                .SubjectNewsfeeds
                .Include(s => s.Newsfeed)
                .ThenInclude(n => n.Author)
                .Where(s => s.SubjectId == subjectId)
                .ToListAsync();
        }

        public Task<List<GuildNewsfeedEntity>> GetGuildNewsfeeds(int guildId)
        {
            return _dbContext
                .GuildNewsfeeds
                .Include(s => s.Newsfeed)
                .ThenInclude(n => n.Author)
                .Where(s => s.GuildId == guildId)
                .ToListAsync();
        }
    }
}