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

        public SubjectActivityGenerator(List<GroupSubject> groupSubjects, List<Student> students)
        {
            SubjectActivityEntities = new List<SubjectActivity>();
            foreach (Student student in students)
            {
                foreach (GroupSubject groupSubjectEntity in groupSubjects.Where(gs => gs.StudyGroupId == student.GroupId))
                {
                    SubjectActivityEntities.Add(new SubjectActivity
                    {
                        GroupSubjectId = groupSubjectEntity.Id,
                        StudentId = student.Id,
                        Points = RandomExtensions.Instance.NextDouble() * 100
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