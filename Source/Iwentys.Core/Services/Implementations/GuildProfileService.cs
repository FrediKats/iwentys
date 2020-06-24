using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Tools;
using Iwentys.Database.Entities;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Transferable.Guilds;
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

        public GuildProfileDto Create(int creator, GuildCreateArgumentDto arguments)
        {
            UserProfile creatorUser = _userProfileRepository.Get(creator);
            
            //TODO: check if user already in guild
            var newGuild = new GuildProfile
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title
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

        public GuildProfileDto[] Get()
        {
            return _guildProfileRepository.Read().Select(GuildProfileDto.Create).ToArray();
        }

        public GuildProfileDto Get(int id)
        {
            return _guildProfileRepository.Get(id).To(GuildProfileDto.Create);
        }
    }
}