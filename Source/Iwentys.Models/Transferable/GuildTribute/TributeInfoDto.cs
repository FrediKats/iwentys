using System;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Students;
using Iwentys.Models.Types.Guilds;
using LanguageExt;

namespace Iwentys.Models.Transferable.GuildTribute
{
    public class TributeInfoDto
    {
        public StudentProjectInfoDto Project { get; set; }

        public int GuildId { get; set; }

        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTime { get; set; }

        public StudentPartialProfileDto Mentor { get; set; }
        public int? MentorId { get; set; }

        public static TributeInfoDto Wrap(TributeEntity project)
        {
            return new TributeInfoDto
            {
                Project = StudentProjectInfoDto.Wrap(project.ProjectEntity),
                GuildId = project.GuildId,
                State = project.State,
                DifficultLevel = project.DifficultLevel,
                Mark = project.Mark,
                CreationTime = project.CreationTime,
                Mentor = project.Mentor.IsNull() ? null : new StudentPartialProfileDto(project.Mentor)
            };
        }
    }
}