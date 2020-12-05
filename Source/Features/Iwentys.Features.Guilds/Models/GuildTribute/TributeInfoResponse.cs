using System;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Models.GuildTribute
{
    public class TributeInfoResponse
    {
        public StudentProjectInfoResponse Project { get; set; }

        public int GuildId { get; set; }

        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTime { get; set; }

        public StudentInfoDto Mentor { get; set; }
        public int? MentorId { get; set; }

        public static TributeInfoResponse Wrap(TributeEntity project)
        {
            return new TributeInfoResponse
            {
                Project = StudentProjectInfoResponse.Wrap(project.ProjectEntity),
                GuildId = project.GuildId,
                State = project.State,
                DifficultLevel = project.DifficultLevel,
                Mark = project.Mark,
                CreationTime = project.CreationTime,
                Mentor = project.Mentor is null ? null : new StudentInfoDto(project.Mentor)
            };
        }
    }
}