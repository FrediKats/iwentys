using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public class StudentActivityInfoDto
    {
        public List<SubjectActivityInfoResponse> Activity { get; set; }
        //public int StudyLeaderBoardPlace { get; set; }
        //public int CodingLeaderBoardPlace { get; set; }
        
        public static StudentActivityInfoDto Wrap(List<SubjectActivityEntity> activity)
        {
            return new StudentActivityInfoDto()
            {
                Activity = activity.SelectToList(s => new SubjectActivityInfoResponse(s))
            };
        }
    }
}