using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services
{
    public class GuildTributeService
    {
        private readonly DatabaseAccessor _database;
        private readonly IGithubApiAccessor _githubApi;

        public GuildTributeService(DatabaseAccessor database, IGithubApiAccessor githubApi)
        {
            _database = database;
            _githubApi = githubApi;
        }

        public TributeInfoResponse[] GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _database.Tribute
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public TributeInfoResponse[] GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _database.Tribute
                .ReadStudentInGuildTributes(guild.Id, user.Id)
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TributeInfoResponse> CreateTribute(AuthorizedUser user, CreateProjectRequest createProject)
        {
            StudentEntity student = _database.Student.Get(user.Id);
            if (student.GithubUsername != createProject.Owner)
                throw InnerLogicException.TributeEx.TributeCanBeSendFromStudentAccount(student, createProject);

            GithubRepository githubProject = _githubApi.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = await _database.StudentProject.GetOrCreate(githubProject, student);
            GuildEntity guild = _database.Guild.ReadForStudent(student.Id);
            TributeEntity[] allTributes = _database.Tribute.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.TributeEx.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.TributeEx.UserAlreadyHaveTribute(user.Id);

            return _database.Tribute.Create(guild, projectEntity).To(TributeInfoResponse.Wrap);
        }

        public async Task<TributeInfoResponse> CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
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

            TributeEntity updatedTribute = await _database.Tribute.Update(tribute);
            return TributeInfoResponse.Wrap(updatedTribute);
        }

        public async Task<TributeInfoResponse> CompleteTribute(AuthorizedUser user, TributeCompleteRequest tributeCompleteRequest)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            TributeEntity tribute = _database.Tribute.Get(tributeCompleteRequest.TributeId);
            GuildMentorUser mentor = student.EnsureIsMentor(_database.Guild, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteRequest.DifficultLevel, tributeCompleteRequest.Mark);
            TributeEntity updatedTribute = await _database.Tribute.Update(tribute);
            return TributeInfoResponse.Wrap(updatedTribute);
        }
    }
}