using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;

namespace Iwentys.Modules.Guilds
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
    }
}