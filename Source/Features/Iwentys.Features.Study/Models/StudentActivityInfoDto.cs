using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record StudentActivityInfoDto
    {
        public StudentActivityInfoDto(List<SubjectActivityEntity> activity)
            : this(activity.SelectToList(s => new SubjectActivityInfoResponseDto(s)))
        {
        }

        public StudentActivityInfoDto(List<SubjectActivityInfoResponseDto> activity) :this()
        {
            Activity = activity;
        }

        private StudentActivityInfoDto()
        {
        }

        public List<SubjectActivityInfoResponseDto> Activity { get; init; }
        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
    }
}