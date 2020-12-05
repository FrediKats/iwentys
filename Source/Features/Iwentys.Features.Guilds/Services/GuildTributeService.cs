using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Repositories;
using Iwentys.Features.GithubIntegration.ViewModels;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.Guilds.ViewModels.GuildTribute;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Guilds.Services
{
    public class GuildTributeService
    {
        private readonly GuildRepositoriesScope _database;
        private readonly IGithubApiAccessor _githubApi;
        private readonly IStudentProjectRepository _studentProjectRepository;

        public GuildTributeService(GuildRepositoriesScope database, IGithubApiAccessor githubApi, IStudentProjectRepository studentProjectRepository)
        {
            _database = database;
            _githubApi = githubApi;
            _studentProjectRepository = studentProjectRepository;
        }

        public TributeInfoResponse[] GetPendingTributes(AuthorizedUser user)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _database.GuildTribute
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public TributeInfoResponse[] GetStudentTributeResult(AuthorizedUser user)
        {
            GuildEntity guild = _database.Guild.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _database.GuildTribute
                .ReadStudentInGuildTributes(guild.Id, user.Id)
                .Select(TributeInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TributeInfoResponse> CreateTribute(AuthorizedUser user, CreateProjectRequest createProject)
        {
            StudentEntity student = await _database.Student.GetAsync(user.Id);
            if (student.GithubUsername != createProject.Owner)
                throw InnerLogicException.TributeEx.TributeCanBeSendFromStudentAccount(student.Id, createProject.Owner);

            GithubRepository githubProject = _githubApi.GetRepository(createProject.Owner, createProject.RepositoryName);
            GithubProjectEntity projectEntity = await _studentProjectRepository.GetOrCreateAsync(githubProject, student);
            GuildEntity guild = _database.Guild.ReadForStudent(student.Id);
            TributeEntity[] allTributes = _database.GuildTribute.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.TributeEx.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.TributeEx.UserAlreadyHaveTribute(user.Id);

            return _database.GuildTribute.Create(guild, projectEntity).To(TributeInfoResponse.Wrap);
        }

        public async Task<TributeInfoResponse> CancelTribute(AuthorizedUser user, long tributeId)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            TributeEntity tribute = await _database.GuildTribute.GetAsync(tributeId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute.ProjectId);

            if (tribute.ProjectEntity.StudentId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                await student.EnsureIsMentor(_database.Guild, tribute.GuildId);
                tribute.SetCanceled();
            }

            TributeEntity updatedTribute = await _database.GuildTribute.UpdateAsync(tribute);
            return TributeInfoResponse.Wrap(updatedTribute);
        }

        public async Task<TributeInfoResponse> CompleteTribute(AuthorizedUser user, TributeCompleteRequest tributeCompleteRequest)
        {
            StudentEntity student = await user.GetProfile(_database.Student);
            TributeEntity tribute = await _database.GuildTribute.GetAsync(tributeCompleteRequest.TributeId);
            GuildMentorUser mentor = await student.EnsureIsMentor(_database.Guild, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute.ProjectId);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteRequest.DifficultLevel, tributeCompleteRequest.Mark);
            TributeEntity updatedTribute = await _database.GuildTribute.UpdateAsync(tribute);
            return TributeInfoResponse.Wrap(updatedTribute);
        }
    }
}