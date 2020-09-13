using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildTributeService : IGuildTributeService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IGithubApiAccessor _githubApi;

        public GuildTributeService(DatabaseAccessor databaseAccessor, IGithubApiAccessor githubApi)
        {
            _databaseAccessor = databaseAccessor;
            _githubApi = githubApi;
        }

        public TributeInfoDto[] GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _databaseAccessor.GuildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _databaseAccessor.TributeRepository
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoDto.Wrap)
                .ToArray();
        }

        public TributeInfoDto[] GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _databaseAccessor.GuildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _databaseAccessor.TributeRepository
                .ReadStudentInGuildTributes(guild.Id, user.Id)
                .Select(TributeInfoDto.Wrap)
                .ToArray();
        }

        public TributeInfoDto CreateTribute(AuthorizedUser user, CreateProjectDto createProject)
        {
            StudentEntity student = _databaseAccessor.Student.Get(user.Id);
            if (student.GithubUsername != createProject.Owner)
                throw InnerLogicException.TributeEx.TributeCanBeSendFromStudentAccount(student, createProject);

            GithubRepository githubProject = _githubApi.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = _databaseAccessor.StudentProjectRepository.GetOrCreate(githubProject, student);
            GuildEntity guild = _databaseAccessor.GuildRepository.ReadForStudent(student.Id);
            TributeEntity[] allTributes = _databaseAccessor.TributeRepository.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.TributeEx.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.TributeEx.UserAlreadyHaveTribute(user.Id);

            return _databaseAccessor.TributeRepository.Create(guild, projectEntity).To(TributeInfoDto.Wrap);
        }

        public TributeInfoDto CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = user.GetProfile(_databaseAccessor.Student);
            TributeEntity tribute = _databaseAccessor.TributeRepository.Get(tributeId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            if (tribute.ProjectEntity.StudentId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                student.EnsureIsMentor(_databaseAccessor.GuildRepository, tribute.GuildId);
                tribute.SetCanceled();
            }

            return _databaseAccessor.TributeRepository.Update(tribute).To(TributeInfoDto.Wrap);
        }

        public TributeInfoDto CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto)
        {
            StudentEntity student = user.GetProfile(_databaseAccessor.Student);
            TributeEntity tribute = _databaseAccessor.TributeRepository.Get(tributeCompleteDto.TributeId);
            GuildMentorUser mentor = student.EnsureIsMentor(_databaseAccessor.GuildRepository, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteDto.DifficultLevel, tributeCompleteDto.Mark);
            return _databaseAccessor.TributeRepository.Update(tribute).To(TributeInfoDto.Wrap);
        }
    }
}