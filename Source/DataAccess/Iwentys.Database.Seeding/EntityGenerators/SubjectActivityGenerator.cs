using System.Collections.Generic;
using System.Linq;

using Iwentys.Database.Seeding.Tools;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectActivityGenerator : IEntityGenerator
    {
        public List<SubjectActivity> SubjectActivityEntities { get; set; }

        public SubjectActivityGenerator(List<GroupSubject> groupSubjects, List<Student> students, List<StudyGroupMember> studyGroupMembers)
        {
            SubjectActivityEntities = new List<SubjectActivity>();
            foreach (StudyGroupMember studyGroupMember in studyGroupMembers)
            {
                foreach (GroupSubject groupSubjectEntity in groupSubjects.Where(gs => gs.StudyGroupId == studyGroupMember.GroupId))
                {
                    SubjectActivityEntities.Add(new SubjectActivity
                    {
                        GroupSubjectId = groupSubjectEntity.Id,
                        StudentId = studyGroupMember.StudentId,
                        Points = RandomExtensions.Instance.Random.Double() * 100
                    });
                }
            }
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectActivity>().HasData(SubjectActivityEntities);
        }
    }
}