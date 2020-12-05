using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectActivityGenerator
    {
        public List<SubjectActivityEntity> SubjectActivityEntities { get; set; }

        public SubjectActivityGenerator(List<GroupSubjectEntity> groupSubjects, List<StudentEntity> students)
        {
            SubjectActivityEntities = new List<SubjectActivityEntity>();
            foreach (StudentEntity student in students)
            {
                foreach (GroupSubjectEntity groupSubjectEntity in groupSubjects.Where(gs => gs.StudyGroupId == student.GroupId))
                {
                    SubjectActivityEntities.Add(new SubjectActivityEntity
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