using System;
using System.Linq.Expressions;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Tributes.Entities;
using Iwentys.Features.Tributes.Enums;

namespace Iwentys.Features.Tributes.Models
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

        public static Expression<Func<TributeEntity, TributeInfoResponse>> FromEntity =>
            project =>
                new TributeInfoResponse
                {
                    Project = StudentProjectInfoResponse.Wrap(project.ProjectEntity),
                    GuildId = project.GuildId,
                    State = project.State,
                    DifficultLevel = project.DifficultLevel,
                    Mark = project.Mark,
                    CreationTimeUtc = project.CreationTimeUtc,
                    Mentor = project.Mentor == null ? null : new StudentInfoDto(project.Mentor)
                };

        public static TributeInfoResponse Wrap(TributeEntity project)
        {
            return new TributeInfoResponse
            {
                Project = StudentProjectInfoResponse.Wrap(project.ProjectEntity),
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