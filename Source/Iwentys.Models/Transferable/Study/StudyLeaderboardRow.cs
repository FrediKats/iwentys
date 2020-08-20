using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Study
{
    public class StudyLeaderboardRow
    {
        public StudyLeaderboardRow(IEnumerable<SubjectActivity> activity)
        {
            List<SubjectActivity> activity1 = activity.ToList();
            Student = activity1.First().Student.To(s => new StudentPartialProfileDto(s));
            Activity = activity1.Sum(a => a.Points);
        }
        
        public StudentPartialProfileDto Student { get; set; }
        public double Activity { get; set; }
    }
}