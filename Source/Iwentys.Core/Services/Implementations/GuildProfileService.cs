using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildProfileService : IGuildProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IGuildProfileRepository _guildProfileRepository;

        public GuildProfileService(IGuildProfileRepository guildProfileRepository, IUserProfileRepository userProfileRepository)
        {
            _guildProfileRepository = guildProfileRepository;
            _userProfileRepository = userProfileRepository;
        }

        public GuildProfileDto Create(int creatorId, GuildCreateArgumentDto arguments)
        {
            UserProfile creatorUser = _userProfileRepository.Get(creatorId);

            var userGuild = _guildProfileRepository.ReadForUser(creatorId);
            if (userGuild != null)
                throw new InnerLogicException("User already in guild");

            var newGuild = new GuildProfile
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

            return _guildProfileRepository.Create(newGuild).To(GuildProfileDto.Create);
        }

        public GuildProfileDto Update(int creator, GuildUpdateArgumentDto arguments)
        {
            //TODO: check permission
            GuildProfile info = _guildProfileRepository.Get(arguments.Id);
            info.Bio = arguments.Bio ?? info.Bio;
            info.LogoUrl = arguments.LogoUrl?? info.LogoUrl;
            info.HiringPolicy = arguments.HiringPolicy ?? info.HiringPolicy;
            return _guildProfileRepository.Update(info).To(GuildProfileDto.Create);
        }

        public GuildProfileDto ApproveGuildCreating(int adminId, int guildId)
        {
            _userProfileRepository
                .Get(adminId)
                .EnsureIsAdmin();

            GuildProfile guild = _guildProfileRepository.Get(guildId);
            if (guild.GuildType == GuildType.Created)
                throw new InnerLogicException("Guild already approved");

            guild.GuildType = GuildType.Created;
            return _guildProfileRepository.Update(guild).To(GuildProfileDto.Create);
        }

        public GuildProfileDto[] Get()
        {
            return _guildProfileRepository.Read().Select(GuildProfileDto.Create).ToArray();
        }

        public GuildProfileDto Get(int id)
        {
            return _guildProfileRepository.Get(id).To(GuildProfileDto.Create);
        }

        public GuildProfileDto GetUserProfile(int userId)
        {
            return _guildProfileRepository.ReadForUser(userId).To(GuildProfileDto.Create);
        }
    }
}