using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildTributeService : IGuildTributeService
    {
        private readonly DatabaseAccessor _database;
        private readonly IGithubApiAccessor _githubApi;

        public GuildTributeService(DatabaseAccessor database, IGithubApiAccessor githubApi)
        {
            _database = database;
            _githubApi = githubApi;
        }

        public TributeInfoDto[] GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _database.Tribute
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoDto.Wrap)
                .ToArray();
        }

        public TributeInfoDto[] GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _database.Tribute
                .ReadStudentInGuildTributes(guild.Id, user.Id)
                .Select(TributeInfoDto.Wrap)
                .ToArray();
        }

        public TributeInfoDto CreateTribute(AuthorizedUser user, CreateProjectDto createProject)
        {
            StudentEntity student = _database.Student.Get(user.Id);
            if (student.GithubUsername != createProject.Owner)
                throw InnerLogicException.TributeEx.TributeCanBeSendFromStudentAccount(student, createProject);

            GithubRepository githubProject = _githubApi.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = _database.StudentProject.GetOrCreate(githubProject, student);
            GuildEntity guild = _database.Guild.ReadForStudent(student.Id);
            TributeEntity[] allTributes = _database.Tribute.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.TributeEx.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.TributeEx.UserAlreadyHaveTribute(user.Id);

            return _database.Tribute.Create(guild, projectEntity).To(TributeInfoDto.Wrap);
        }

        public TributeInfoDto CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            TributeEntity tribute = _database.Tribute.Get(tributeId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            if (tribute.ProjectEntity.StudentId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                student.EnsureIsMentor(_database.Guild, tribute.GuildId);
                tribute.SetCanceled();
            }

            return _database.Tribute.Update(tribute).To(TributeInfoDto.Wrap);
        }

        public TributeInfoDto CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto)
        {
            StudentEntity student = user.GetProfile(_database.Student);
            TributeEntity tribute = _database.Tribute.Get(tributeCompleteDto.TributeId);
            GuildMentorUser mentor = student.EnsureIsMentor(_database.Guild, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteDto.DifficultLevel, tributeCompleteDto.Mark);
            return _database.Tribute.Update(tribute).To(TributeInfoDto.Wrap);
        }
    }
}