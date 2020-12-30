using System;
using System.Linq.Expressions;
using Iwentys.Features.Guilds.Tributes.Entities;
using Iwentys.Features.Guilds.Tributes.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Guilds.Tributes.Models
{
    public class TributeInfoResponse
    {
        public StudentProjectInfoResponse Project { get; set; }

        public int GuildId { get; set; }

        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public StudentInfoDto Mentor { get; set; }
        public int? MentorId { get; set; }

        public static Expression<Func<Tribute, TributeInfoResponse>> FromEntity =>
            project =>
                new TributeInfoResponse
                {
                    Project = new StudentProjectInfoResponse
                    {
                        Id = project.Project.Id,
                        Url = project.Project.FullUrl,
                        Name = project.Project.FullUrl,
                        Description = project.Project.Description
                    },

                    GuildId = project.GuildId,
                    State = project.State,
                    DifficultLevel = project.DifficultLevel,
                    Mark = project.Mark,
                    CreationTimeUtc = project.CreationTimeUtc,
                    Mentor = project.Mentor == null ? null : new StudentInfoDto(project.Mentor)
                };

        public static TributeInfoResponse Wrap(Tribute project)
        {
            return new TributeInfoResponse
            {
                Project = StudentProjectInfoResponse.Wrap(project.Project),
                GuildId = project.GuildId,
                State = project.State,
                DifficultLevel = project.DifficultLevel,
                Mark = project.Mark,
                CreationTimeUtc = project.CreationTimeUtc,
                Mentor = project.Mentor is null ? null : new StudentInfoDto(project.Mentor)
            };
        }
    }
}