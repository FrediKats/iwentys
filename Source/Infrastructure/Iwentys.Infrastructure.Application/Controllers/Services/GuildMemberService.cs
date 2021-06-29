using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Infrastructure.Application.Controllers.Services
{
    public class GuildMemberService
    {
        private readonly IGenericRepository<GuildMember> _guildMemberRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<GuildLastLeave> _guildLastLeaveRepository;
        private readonly IGenericRepository<IwentysUser> _userRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GuildMemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
            _guildLastLeaveRepository = _unitOfWork.GetRepository<GuildLastLeave>();
        }

        public async Task RequestGuild(AuthorizedUser authorizedUser, int guildId)
        {
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser user = await _userRepository.GetById(authorizedUser.Id);
            GuildLastLeave lastLeave = await _guildLastLeaveRepository.FindByIdAsync(user.Id);
            GuildMember guildMember = _guildMemberRepository
                .Get()
                .FirstOrDefault(m => m.Member.Id == authorizedUser.Id && m.MemberType == GuildMemberType.Requested);

            GuildMember newMembership = guild.RequestEnterGuild(user, guildMember, lastLeave);

            _guildMemberRepository.Insert(newMembership);
            await _unitOfWork.CommitAsync();
        }
            
        public async Task<GuildMember[]> GetGuildRequests(AuthorizedUser user, int guildId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);

            student.EnsureIsGuildMentor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Requested)
                .ToArray();
        }

        public async Task<GuildMember[]> GetGuildBlocked(AuthorizedUser user, int guildId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);

            student.EnsureIsGuildMentor(guild);

            return guild.Members
                .Where(m => m.MemberType == GuildMemberType.Blocked)
                .ToArray();
        }

        public async Task BlockGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            IwentysUser editorStudentAccount = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMember memberToKick = guild.EnsureMemberCanRestrictPermissionForOther(editorStudentAccount, memberId);
            IwentysUser iwentysUser = await _userRepository.GetById(user.Id);
            GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _guildLastLeaveRepository);
            
            memberToKick.MarkBlocked(guildLastLeave);
            
            _guildMemberRepository.Update(memberToKick);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnblockStudent(AuthorizedUser user, int guildId, int studentId)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser iwentysUser = await _userRepository.GetById(studentId);
            GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _guildLastLeaveRepository);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Blocked)
                throw new InnerLogicException($"Student is not blocked in guild! AuthorId: {studentId} GuildId: {guildId}");

            guild.RemoveMember(student, iwentysUser, guildLastLeave);
            await _unitOfWork.CommitAsync();
        }

        public async Task KickGuildMember(AuthorizedUser user, int guildId, int memberId)
        {
            IwentysUser editorStudentAccount = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser iwentysUser = await _userRepository.GetById(memberId);
            GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _guildLastLeaveRepository);

            guild.RemoveMember(editorStudentAccount, iwentysUser, guildLastLeave);
            await _unitOfWork.CommitAsync();
        }

        public async Task AcceptRequest(IwentysUser user, int guildId, int memberForAccepting)
        {
            await AcceptRequest(AuthorizedUser.DebugAuth(user.Id), guildId, memberForAccepting);
        }

        public async Task AcceptRequest(AuthorizedUser user, int guildId, int memberForAccepting)
        {
            IwentysUser student = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            GuildMember member = guild.Members.Find(m => m.MemberId == memberForAccepting) ?? throw EntityNotFoundException.Create(typeof(GuildMember), memberForAccepting);

            GuildMentor guildMentor = student.EnsureIsGuildMentor(guild);
            member.Approve(guildMentor);

            _guildMemberRepository.Update(member);
            await _unitOfWork.CommitAsync();
        }

        public async Task RejectRequest(AuthorizedUser user, int guildId, int studentId)
        {
            IwentysUser initiator = await _userRepository.GetById(user.Id);
            Guild guild = await _guildRepository.GetById(guildId);
            IwentysUser iwentysUser = await _userRepository.GetById(studentId);
            GuildLastLeave guildLastLeave = await GuildRepository.Get(iwentysUser, _guildLastLeaveRepository);

            GuildMember member = guild.Members.Find(m => m.MemberId == studentId);

            if (member is null || member.MemberType != GuildMemberType.Requested)
                throw InnerLogicException.GuildExceptions.RequestWasNotFound(studentId, guildId);

            guild.RemoveMember(initiator, iwentysUser, guildLastLeave);
            await _unitOfWork.CommitAsync();
        }

        public async Task PromoteToMentor(IwentysUser creator, int guildId, int userForPromotion)
        {
            await PromoteToMentor(AuthorizedUser.DebugAuth(creator.Id), guildId, userForPromotion);
        }

        public async Task PromoteToMentor(AuthorizedUser creator, int guildId, int userForPromotion)
        {
            IwentysUser studentCreator = await _userRepository.GetById(creator.Id);
            Guild guild = await _guildRepository.FindByIdAsync(guildId);
            GuildCreator guildCreator = GuildCreatorExtensions.EnsureIsCreator(studentCreator, guild);

            GuildMember studentMembership = _guildMemberRepository
                .Get()
                .Where(GuildMember.IsMember())
                .Single(gm => gm.MemberId == userForPromotion && gm.GuildId == guildId);
            studentMembership.MakeMentor(guildCreator);

            _guildMemberRepository.Update(studentMembership);
            await _unitOfWork.CommitAsync();
        }
    }
}