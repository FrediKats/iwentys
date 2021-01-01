using System.Collections.Generic;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class AssignmentGenerator : IEntityGenerator
    {
        public List<Assignment> Assignments { get; set; }
        public List<StudentAssignment> StudentAssignments { get; set; }

        public AssignmentGenerator(List<Student> students)
        {
            var faker = new Faker();
            var assignmentCreateRequestFaker = new AssignmentCreateRequestFaker();

            Assignments = new List<Assignment>();
            StudentAssignments = new List<StudentAssignment>();
            foreach (Student student in students)
            {
                var assignmentEntity = Assignment.Create(student, assignmentCreateRequestFaker.Generate());
                
                assignmentEntity.Id = 1 + faker.IndexVariable++;
                Assignments.Add(assignmentEntity);
                StudentAssignments.Add(new StudentAssignment
                {
                    AssignmentId = assignmentEntity.Id,
                    StudentId = student.Id,
                });
            }
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>().HasData(Assignments);
            modelBuilder.Entity<StudentAssignment>().HasData(StudentAssignments);
        }
    }
}