using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding;

public class SubjectActivityGenerator : IEntityGenerator
{
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
                    Points = RandomExtensions.Instance.Random.Double() * 100
                });
            }
        }
    }

    public List<SubjectActivity> SubjectActivityEntities { get; set; }

    public void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SubjectActivity>().HasData(SubjectActivityEntities);
    }
}