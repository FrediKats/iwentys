using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types.Github;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildService : IGuildService
    {
        private readonly IGithubApiAccessor _apiAccessor;

        private readonly IGuildRepository _guildRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITributeRepository _tributeRepository;
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IStudentProjectRepository _studentProjectRepository;

        public GuildService(IGuildRepository guildRepository,
            IStudentRepository studentRepository,
            IStudentProjectRepository studentProjectRepository,
            ITributeRepository tributeRepository,
            DatabaseAccessor databaseAccessor, IGithubApiAccessor apiAccessor)
        {
            _guildRepository = guildRepository;
            _studentRepository = studentRepository;
            _studentProjectRepository = studentProjectRepository;
            _tributeRepository = tributeRepository;
            _databaseAccessor = databaseAccessor;
            _apiAccessor = apiAccessor;
        }

        public GuildProfileDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments)
        {
            Student creatorUser = _studentRepository.Get(creator.Id);

            Guild userGuild = _guildRepository.ReadForStudent(creatorUser.Id);
            if (userGuild != null)
                throw new InnerLogicException("Student already in guild");

            var newGuild = new Guild
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members = new List<GuildMember>
            {
                new GuildMember {Guild = newGuild, Member = creatorUser, MemberType = GuildMemberType.Creator}
            };

            return _guildRepository.Create(newGuild).To(ToGuildProfileDto);
        }

        public GuildProfileDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments)
        {
            //TODO: check permission
            Guild info = _guildRepository.Get(arguments.Id);
            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;
            return _guildRepository.Update(info).To(ToGuildProfileDto);
        }

        public GuildProfileDto ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            _studentRepository
                .Get(user.Id)
                .EnsureIsAdmin();

            Guild guild = _guildRepository.Get(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            return _guildRepository.Update(guild).To(ToGuildProfileDto);
        }

        public GuildProfileDto[] Get()
        {
            return _guildRepository.Read().AsEnumerable().Select(ToGuildProfileDto).ToArray();
        }

        public GuildProfileDto Get(int id)
        {
            return _guildRepository.Get(id).To(ToGuildProfileDto);
        }

        public GuildProfileDto GetStudentGuild(int userId)
        {
            return _guildRepository.ReadForStudent(userId).To(ToGuildProfileDto);
        }

        public VotingInfoDto StartVotingForLeader(AuthorizedUser user, int guildId, GuildLeaderVotingCreateDto votingCreateDto)
        {
            throw new System.NotImplementedException();
        }

        public VotingInfoDto StartVotingForTotem(AuthorizedUser user, int guildId, GuildTotemVotingCreateDto votingCreateDto)
        {
            throw new System.NotImplementedException();
        }

        public void SetTotem(AuthorizedUser user, int guildId, int totemId)
        {
            //TODO: ensure user is not totem in other guilds
            user.EnsureIsAdmin();
            Student totem = _studentRepository.Get(totemId);
            Guild guild = _guildRepository.Get(guildId);

            if (guild.TotemId != null)
                throw new InnerLogicException("Guild already has totem.");

            guild.TotemId = totem.Id;
            _guildRepository.Update(guild);
        }

        public Tribute[] GetPendingTributes(AuthorizedUser user)
        {
            Guild guild = _guildRepository.ReadForTotem(user.Id) ?? throw new InnerLogicException("User is not totem in any guild");

            return _tributeRepository
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Pending)
                .ToArray();
        }

        public Tribute[] GetStudentTributeResult(AuthorizedUser user)
        {
            Guild guild = _guildRepository.ReadForStudent(user.Id);
            if (guild is null)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id);

            return _tributeRepository.ReadStudentInGuildTributes(guild.Id, user.Id);
        }

        public Tribute CreateTribute(AuthorizedUser user, int projectId)
        {
            Student student = _studentRepository.Get(user.Id);
            Guild guild = _guildRepository.ReadForStudent(student.Id);
            StudentProject project = _studentProjectRepository.Get(projectId);
            Tribute[] allTributes = _tributeRepository.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectId))
                throw new InnerLogicException("Repository already used for tribute");

            if (allTributes.Any(t => t.State == TributeState.Pending && t.Project.StudentId == student.Id))
                throw new InnerLogicException("Other tribute already created and waiting for check");

            if (guild.TotemId is null)
                throw new InnerLogicException("Can't send tribute. There is no totem in guild");

            var tribute = Tribute.New(guild.Id, project.Id);
            return _tributeRepository.Create(tribute);
        }

        public Tribute CancelTribute(AuthorizedUser user, int tributeId)
        {
            Tribute tribute = _tributeRepository.Get(tributeId);

            if (tribute.State != TributeState.Pending)
                throw new InnerLogicException($"Can't cancel tribute. It's state is: {tribute.State}");

            if (tribute.Project.StudentId == user.Id)
                tribute.SetCanceled();
            else
            {
                user.EnsureIsTotem(_guildRepository, tribute.GuildId);
                tribute.SetCanceled();
            }
            
            return _tributeRepository.Update(tribute);
        }

        public Tribute CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto)
        {
            Tribute tribute = _tributeRepository.Get(tributeCompleteDto.TributeId);
            GuildTotemUser totem = user.EnsureIsTotem(_guildRepository, tribute.GuildId);

            if (tribute.State != TributeState.Pending)
                throw new InnerLogicException($"Can't complete tribute. It's state is: {tribute.State}");

            tribute.SetCompleted(totem.Student.Id, tributeCompleteDto.DifficultLevel, tribute.Mark);
            return _tributeRepository.Update(tribute);
        }

        public GithubRepository AddPinnedRepository(AuthorizedUser user, int guildId, string repositoryUrl)
        {
            throw new System.NotImplementedException();
        }

        public GithubRepository DeletePinnedRepository(AuthorizedUser user, int guildId, string repositoryUrl)
        {
            throw new System.NotImplementedException();
        }

        public GuildProfileDto ToGuildProfileDto(Guild profile)
        {
            return new GuildProfileDto
            {
                Id = profile.Id,
                Bio = profile.Bio,
                HiringPolicy = profile.HiringPolicy,
                LogoUrl = profile.LogoUrl,
                Title = profile.Title,
                Totem = profile.Totem,
                Members = profile.Members.Select(m => m.Member).ToList(),
                PinnedRepositories = profile.PinnedProjects.SelectToList(p => _apiAccessor.GetRepository(p.RepositoryOwner, p.RepositoryName))
            };
        }
    }
}