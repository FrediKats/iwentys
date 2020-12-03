using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Repositories;
using Iwentys.Features.Newsfeeds.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Newsfeeds
{
    public class NewsfeedRepository : INewsfeedRepository
    {
        private readonly IwentysDbContext _dbContext;

        public NewsfeedRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SubjectNewsfeedEntity> CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, int authorId, int subjectId)
        {
            EntityEntry<NewsfeedEntity> newsfeedEntity = await _dbContext.Newsfeeds.AddAsync(new NewsfeedEntity
            {
                Title = createViewModel.Title,
                Content = createViewModel.Content,
                CreationTimeUtc = DateTime.UtcNow,
                AuthorId = authorId
            });

            EntityEntry<SubjectNewsfeedEntity> subjectNewsfeed = await _dbContext.SubjectNewsfeeds.AddAsync(new SubjectNewsfeedEntity
            {
                NewsfeedId = newsfeedEntity.Entity.Id,
                SubjectId = subjectId
            });

            await _dbContext.SaveChangesAsync();

            SubjectNewsfeedEntity result = subjectNewsfeed.Entity;
            result.Newsfeed = newsfeedEntity.Entity;
            return result;
        }

        public Task<List<SubjectNewsfeedEntity>> ReadSubjectNewsfeeds(int subjectId)
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