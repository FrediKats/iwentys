using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectActivityGenerator
    {
        public List<SubjectActivity> SubjectActivityEntities { get; set; }

        public SubjectActivityGenerator(List<GroupSubject> groupSubjects, List<Student> students)
        {
            SubjectActivityEntities = new List<SubjectActivity>();
            foreach (Student student in students)
            {
                foreach (GroupSubject groupSubjectEntity in groupSubjects.Where(gs => gs.StudyGroupId == student.GroupId))
                {
                    SubjectActivityEntities.Add(new SubjectActivity
                    {
                        GroupSubjectEntityId = groupSubjectEntity.Id,
                        StudentId = student.Id,
                        Points = RandomExtensions.Instance.NextDouble() * 100
                    });
                }
            }
        }
    }
}