using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.StudentFeature.Models
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