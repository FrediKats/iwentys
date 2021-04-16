using System.Collections.Generic;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Domain;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class AssignmentGenerator : IEntityGenerator
    {
        private readonly Faker _faker;

        public AssignmentGenerator(List<Student> students)
        {
            _faker = new Faker();

            Assignments = new List<Assignment>();
            StudentAssignments = new List<StudentAssignment>();
            foreach (Student student in students)
            {
                Assignment assignment = GenerateAssignment(student);
                Assignments.Add(assignment);
                StudentAssignments.Add(new StudentAssignment
                {
                    AssignmentId = assignment.Id,
                    StudentId = student.Id
                });
            }
        }

        public List<Assignment> Assignments { get; set; }
        public List<StudentAssignment> StudentAssignments { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>().HasData(Assignments);
            modelBuilder.Entity<StudentAssignment>().HasData(StudentAssignments);
        }

        public Assignment GenerateAssignment(Student author)
        {
            var assignment = Assignment.Create(author, AssignmentFaker.Instance.CreateAssignmentCreateArguments());
            assignment.Id = 1 + _faker.IndexVariable++;
            return assignment;
        }
    }
}