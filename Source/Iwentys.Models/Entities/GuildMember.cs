using System.ComponentModel.DataAnnotations;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class GuildMember
    {
        public int GuildId { get; set; }
        public Guild Guild { get; set; }

        public int MemberId { get; set; }
        public UserProfile Member { get; set; }

        [Required]
        public GuildMemberType MemberType { get; set; }
    }
}