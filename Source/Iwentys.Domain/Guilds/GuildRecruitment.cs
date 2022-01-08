using System.Collections.Generic;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds.Models;

namespace Iwentys.Domain.Guilds
{
    public class GuildRecruitment
    {
        public int Id { get; init; }

        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public string Description { get; init; }
        public bool IsActive { get; set; }
        
        public virtual List<GuildRecruitmentMember> RecruitmentMembers { get; init; }

        public static GuildRecruitment Create(IwentysUser user, Guild guild, GuildRecruitmentCreateArguments createArguments)
        {
            user.EnsureIsGuildMentor(guild);

            return new GuildRecruitment
            {
                GuildId = guild.Id,
                AuthorId = user.Id,
                Description = createArguments.Description,
                IsActive = true
            };
        }

        public void Close(GuildMentor mentor)
        {
            if (!IsActive)
                throw new InnerLogicException("Already closed");

            IsActive = false;
        }
    }
}