using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GuildRepository : IGuildRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<GuildEntity> Read()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .ThenInclude(gm => gm.Member)
                .Include(g => g.PinnedProjects)
                .Include(g => g.Achievements)
                .ThenInclude(a => a.Achievement)
                .Where(g => g.GuildType == GuildType.Created);
        }

        public GuildEntity ReadById(int key)
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .ThenInclude(gm => gm.Member)
                .Include(g => g.PinnedProjects)
                .Include(g => g.Achievements)
                .ThenInclude(a => a.Achievement)
                .FirstOrDefault(g => g.Id == key);
        }

        public GuildEntity Update(GuildEntity entity)
        {
            EntityEntry<GuildEntity> createdEntity = _dbContext.Guilds.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            GuildEntity user = this.Get(key);
            _dbContext.Guilds.Remove(user);
            _dbContext.SaveChanges();
        }

        public GuildEntity Create(StudentEntity creator, GuildCreateArgumentDto arguments)
        {
            var newGuild = new GuildEntity
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members = new List<GuildMemberEntity>
            {
                new GuildMemberEntity(newGuild, creator, GuildMemberType.Creator)
            };

            EntityEntry<GuildEntity> createdEntity = _dbContext.Guilds.Add(newGuild);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public GuildEntity[] ReadPending()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .Include(g => g.PinnedProjects)
                .Where(g => g.GuildType == GuildType.Pending)
                .ToArray();
        }

        public GuildEntity ReadForStudent(int studentId)
        {
            return _dbContext.GuildMembers
                .Where(gm => gm.MemberId == studentId)
                .WhereIsMember()
                .Include(gm => gm.Guild.Members)
                .Include(gm => gm.Guild.PinnedProjects)
                .Select(gm => gm.Guild)
                .SingleOrDefault();
        }

        public Boolean IsStudentHaveRequest(Int32 studentId)
        {
            return !_dbContext.GuildMembers
                .Where(m=> m.Member.Id == studentId)
                .Any(m => m.MemberType == GuildMemberType.Requested);
        }

        public GuildMemberEntity AddMember(GuildEntity guild, StudentEntity student, GuildMemberType memberType)
        {
            EntityEntry<GuildMemberEntity> addedEntity = _dbContext.GuildMembers.Add(new GuildMemberEntity(guild, student, memberType));
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public GuildMemberEntity UpdateMember(GuildMemberEntity member)
        {
            EntityEntry<GuildMemberEntity> updatedEntity =  _dbContext.GuildMembers.Update(member);

            _dbContext.SaveChanges();

            return updatedEntity.Entity;
        }

        public void RemoveMember(int guildId, int userId)
        {
            GuildMemberEntity guildMember = _dbContext.GuildMembers.Single(gm => gm.GuildId == guildId && gm.MemberId == userId);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.CreatorCannotLeave(userId, guildId);
            _dbContext.GuildMembers.Remove(guildMember);

            _dbContext.SaveChanges();
        }

        public GuildPinnedProjectEntity PinProject(int guildId, string owner, string projectName)
        {
            EntityEntry<GuildPinnedProjectEntity> entry = _dbContext.GuildPinnedProjects.Add(new GuildPinnedProjectEntity
            {
                GuildId = guildId,
                RepositoryName = projectName,
                RepositoryOwner = owner
            });
            _dbContext.SaveChanges();
            return entry.Entity;
        }

        public void UnpinProject(int pinnedProjectId)
        {
            _dbContext.GuildPinnedProjects.Remove(_dbContext.GuildPinnedProjects.Find(pinnedProjectId));
            _dbContext.SaveChanges();
        }
    }
}