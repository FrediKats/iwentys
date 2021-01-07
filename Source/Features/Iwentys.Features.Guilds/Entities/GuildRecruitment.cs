using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Models.Recruitment;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitment
    {
        public int Id { get; init; }

        public int AuthorId { get; set; }
        public IwentysUser Author { get; set; }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public string Description { get; init; }
        public bool IsActive { get; set; }
        
        public virtual List<GuildRecruitmentMember> RecruitmentMembers { get; init; }

        public static GuildRecruitment Create(GuildMentor mentor, Guild guild, GuildRecruitmentCreateArguments createArguments)
        {
            return new GuildRecruitment
            {
                GuildId = guild.Id,
                AuthorId = mentor.User.Id,
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