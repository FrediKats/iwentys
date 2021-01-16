using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding.FakerEntities.Study;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.SubjectAssignments.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectAssignmentGenerator : IEntityGenerator
    {
        public List<SubjectAssignment> SubjectAssignments { get; set; }
        public List<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public List<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }

        public SubjectAssignmentGenerator(
            List<Student> students,
            List<StudyGroup> groups,
            List<Subject> subjects,
            AssignmentGenerator assignmentGenerator)
        {
            SubjectAssignments = new List<SubjectAssignment>();
            GroupSubjectAssignments = new List<GroupSubjectAssignment>();
            SubjectAssignmentSubmits = new List<SubjectAssignmentSubmit>();

            var author = students.First();

            foreach (Subject subject in subjects)
            {
                Assignment assignment = assignmentGenerator.GenerateAssignment(author);
                assignmentGenerator.Assignments.Add(assignment);
                SubjectAssignment sa = SubjectAssignmentFaker.Instance.Create(subject.Id, assignment);
                SubjectAssignments.Add(sa);

                foreach (StudyGroup studyGroup in groups)
                {
                    GroupSubjectAssignments.Add(new GroupSubjectAssignment { GroupId = studyGroup.Id, SubjectAssignmentId = sa.Id });
                }

                foreach (Student student in students)
                {
                    SubjectAssignmentSubmits.Add(SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmit(sa.Id, student.Id));
                }
            }
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectAssignment>().HasData(SubjectAssignments);
            modelBuilder.Entity<GroupSubjectAssignment>().HasData(GroupSubjectAssignments);
            modelBuilder.Entity<SubjectAssignmentSubmit>().HasData(SubjectAssignmentSubmits);
        }
    }
}