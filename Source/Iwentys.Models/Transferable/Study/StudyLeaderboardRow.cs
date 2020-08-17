using System.Collections.Generic;
using System.Linq;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Study
{
    public class StudyLeaderboardRow
    {
        private readonly List<SubjectActivity> _activity;

        public StudyLeaderboardRow(IEnumerable<SubjectActivity> activity)
        {
            _activity = activity.ToList();
            Student = _activity.First().Student.To(s => new StudentPartialProfileDto(s));
            Activity = _activity.Sum(a => a.Points);
        }

        
        public StudentPartialProfileDto Student { get; set; }
        public double Activity { get; set; }
    }
}