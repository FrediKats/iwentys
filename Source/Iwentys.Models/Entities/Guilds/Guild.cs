using System.Collections.Generic;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Guilds
{
    public class Guild
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; set; }

        public UserProfile Totem { get; set; }
        public int TotemId { get; set; }

        public List<GuildMember> Members { get; set; }
    }
}