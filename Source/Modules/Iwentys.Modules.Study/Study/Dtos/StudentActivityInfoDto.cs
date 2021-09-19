using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Domain.Study;

namespace Iwentys.Modules.Study.Study.Dtos
{
    public record StudentActivityInfoDto
    {
        public StudentActivityInfoDto(List<SubjectActivity> activity)
            : this(activity.SelectToList(s => new SubjectActivityInfoResponseDto(s)))
        {
        }

        public StudentActivityInfoDto(List<SubjectActivityInfoResponseDto> activity) : this()
        {
            Activity = activity;
        }

        public StudentActivityInfoDto()
        {
        }

        public List<SubjectActivityInfoResponseDto> Activity { get; init; }
        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
    }
}