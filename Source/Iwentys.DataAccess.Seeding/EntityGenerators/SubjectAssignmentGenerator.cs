using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.DataAccess.Seeding
{
    public class SubjectAssignmentGenerator : IEntityGenerator
    {
        public SubjectAssignmentGenerator(
            List<Student> students,
            List<StudyGroup> groups,
            List<Subject> subjects)
        {
            SubjectAssignments = new List<SubjectAssignment>();
            GroupSubjectAssignments = new List<GroupSubjectAssignment>();
            SubjectAssignmentSubmits = new List<SubjectAssignmentSubmit>();

            Student author = students.First();
            int mentorId = author.Id;

            foreach (Subject subject in subjects.Take(2))
            {
                for (int assignmentPerSubjectCount = 0; assignmentPerSubjectCount < 3; assignmentPerSubjectCount++)
                {
                    SubjectAssignment subjectAssignment = SubjectAssignmentFaker.Instance.Create(subject.Id, mentorId);
                    SubjectAssignments.Add(subjectAssignment);

                    foreach (StudyGroup studyGroup in groups)
                        GroupSubjectAssignments.Add(new GroupSubjectAssignment { GroupId = studyGroup.Id, SubjectAssignmentId = subjectAssignment.Id });

                    foreach (Student student in students)
                    {
                        if (RandomExtensions.Instance.Random.Int(0, 10) < 5)
                        {
                            SubjectAssignmentSubmits.Add(SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmitWithFeedback(subjectAssignment.Id, student.Id));

                        }
                        else
                        {
                            SubjectAssignmentSubmits.Add(SubjectAssignmentFaker.Instance.CreateSubjectAssignmentSubmit(subjectAssignment.Id, student.Id));
                        }
                    }
                }
            }
        }

        public List<SubjectAssignment> SubjectAssignments { get; set; }
        public List<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public List<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectAssignment>().HasData(SubjectAssignments);
            modelBuilder.Entity<GroupSubjectAssignment>().HasData(GroupSubjectAssignments);
            modelBuilder.Entity<SubjectAssignmentSubmit>().HasData(SubjectAssignmentSubmits);
        }
    }
}