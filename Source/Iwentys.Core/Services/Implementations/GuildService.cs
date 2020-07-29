using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.DomainModel.Guilds;
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

        public GuildProfileShortInfoDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments)
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

            return _guildRepository.Create(newGuild)
                .To(g => new GuildDomain(g, _databaseAccessor, _apiAccessor))
                .ToGuildProfileShortInfoDto();
        }

        public GuildProfileShortInfoDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments)
        {
            Guild info = _guildRepository.Get(arguments.Id);
            GuildEditor editor = user.EnsureIsGuildEditor(info);

            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl ?? info.LogoUrl;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;

            if (arguments.HiringPolicy == GuildHiringPolicy.Open)
                foreach (var guildMember in info.Members)
                    if (guildMember.MemberType == GuildMemberType.Requested)
                        guildMember.MemberType = GuildMemberType.Member;

            return _guildRepository.Update(info)
                .To(g => new GuildDomain(g, _databaseAccessor, _apiAccessor))
                .ToGuildProfileShortInfoDto();
        }

        public GuildProfileShortInfoDto ApproveGuildCreating(AuthorizedUser user, int guildId)
        {
            _studentRepository
                .Get(user.Id)
                .EnsureIsAdmin();

            Guild guild = _guildRepository.Get(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            return _guildRepository.Update(guild)
                .To(g => new GuildDomain(g, _databaseAccessor, _apiAccessor))
                .ToGuildProfileShortInfoDto();
        }

        public GuildProfileDto[] Get()
        {
            return _guildRepository.Read().AsEnumerable().Select(g =>
                new GuildDomain(g, _databaseAccessor, _apiAccessor)
                    .ToGuildProfileDto()).ToArray();
        }

        public GuildProfileDto Get(int id, int? userId)
        {
            return _guildRepository.Get(id)
                .To(g => new GuildDomain(g, _databaseAccessor, _apiAccessor))
                .ToGuildProfileDto(userId);
        }

        public GuildProfileDto GetStudentGuild(int userId)
        {
            return _guildRepository.ReadForStudent(userId).To(g =>
                    new GuildDomain(g, _databaseAccessor, _apiAccessor))
                .ToGuildProfileDto(userId);
        }

        public GuildProfileDto EnterGuild(AuthorizedUser user, Int32 guildId)
        {
            GuildDomain guild = _guildRepository.Get(guildId).To(g =>
                new GuildDomain(g, _databaseAccessor, _apiAccessor));

            if (guild.GetUserMembershipState(user.Id) != UserMembershipState.CanEnter)
                throw new InnerLogicException($"Student unable to enter this guild! UserId: {user.Id} GuildId: {guildId}");

            _guildRepository.AddMember(GuildMember.NewMember(guildId, user.Id));

            return Get(guildId, user.Id);
        }

        public GuildProfileDto RequestGuild(AuthorizedUser user, Int32 guildId)
        {
            GuildDomain guild = _guildRepository.Get(guildId).To(g =>
                new GuildDomain(g, _databaseAccessor, _apiAccessor));

            if (guild.GetUserMembershipState(user.Id) != UserMembershipState.CanRequest)
                throw new InnerLogicException($"Student unable to send request to this guild! UserId: {user.Id} GuildId: {guildId}");

            _guildRepository.AddMember(GuildMember.NewRequest(guildId, user.Id));

            return Get(guildId, user.Id);
        }

        public GuildProfileDto LeaveGuild(AuthorizedUser user, int guildId)
        {
            Guild studentGuild = _guildRepository.ReadForStudent(user.Id);
            if (studentGuild == null || studentGuild.Id != guildId)
                throw InnerLogicException.Guild.IsNotGuildMember(user.Id);

            //TODO: we need transaction with rollback on fail
            Tribute userTribute = _tributeRepository.ReadStudentActiveTribute(studentGuild.Id, user.Id);
            if (userTribute != null)
                _tributeRepository.Delete(userTribute.ProjectId);

            _guildRepository.RemoveMember(guildId, user.Id);

            return Get(guildId, user.Id);
        }

        public GuildMember[] GetGuildRequests(AuthorizedUser user, Int32 guildId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public GuildMember[] GetGuildBlocked(AuthorizedUser user, Int32 guildId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public void BlockGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == memberId);
            GuildMember userMember = guild.Members.Find(m => m.MemberId == user.Id);

            if (member is null || !member.MemberType.IsMember())
                throw InnerLogicException.Guild.IsNotGuildMember(memberId);

            if (member.MemberType == GuildMemberType.Creator)
                throw new InnerLogicException("Unable to block guild creator!");

            if (member.MemberType == GuildMemberType.Mentor && userMember.MemberType == GuildMemberType.Mentor)
                throw new InnerLogicException("Mentor unable to kick mentor!");

            member.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();

            member.MemberType = GuildMemberType.Blocked;
            _guildRepository.UpdateMember(member);
        }

        public void UnblockStudent(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! StudentId: {studentId} GuildId: {guildId}");

            _guildRepository.RemoveMember(guildId, studentId);
        }

        public void KickGuildMember(AuthorizedUser user, Int32 guildId, Int32 memberId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == memberId);
            GuildMember userMember = guild.Members.Find(m => m.MemberId == user.Id);

            if (member is null || !member.MemberType.IsMember())
                throw InnerLogicException.Guild.IsNotGuildMember(memberId);

            if (member.MemberType == GuildMemberType.Creator)
                throw new InnerLogicException("Unable to kick guild creator!");

            if (member.MemberType == GuildMemberType.Mentor && userMember.MemberType == GuildMemberType.Mentor)
                throw new InnerLogicException("Mentor unable to kick mentor!");

            member.Member.GuildLeftTime = DateTime.UtcNow.ToUniversalTime();

            _guildRepository.RemoveMember(guildId, memberId);
        }

        public void AcceptRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw new InnerLogicException(
                    $"No request from student to guild! StudentId: {studentId} GuildId: {guildId}");

            member.MemberType = GuildMemberType.Member;

            _guildRepository.UpdateMember(member);
        }

        public void RejectRequest(AuthorizedUser user, Int32 guildId, Int32 studentId)
        {
            Guild guild = _guildRepository.Get(guildId);
            GuildEditor editor = user.EnsureIsGuildEditor(guild);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw new InnerLogicException(
                    $"No request from student to guild! StudentId: {studentId} GuildId: {guildId}");

            _guildRepository.RemoveMember(guildId, studentId);
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
            user.EnsureIsAdmin();
            Student totem = _studentRepository.Get(totemId);
            Guild guild = _guildRepository.Get(guildId);

            if (guild.TotemId != null)
                throw new InnerLogicException("Guild already has totem.");

            if (_guildRepository.ReadForTotem(totemId) != null)
                throw new InnerLogicException("Member is already totem in other guild.");

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
    }
}