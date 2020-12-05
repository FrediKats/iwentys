using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record StudentActivityInfoDto(List<SubjectActivityInfoResponseDto> Activity)
    {
        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }

        public StudentActivityInfoDto(List<SubjectActivityEntity> activity)
            : this(activity.SelectToList(s => new SubjectActivityInfoResponseDto(s)))
        {
        }
    }
}