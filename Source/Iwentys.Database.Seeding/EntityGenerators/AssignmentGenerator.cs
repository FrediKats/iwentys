using System;
using System.Collections.Generic;
using Bogus;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
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

            Assignments = new List<Assignment>();
            StudentAssignments = new List<StudentAssignment>();
            foreach (Student student in students)
            {
                var assignmentEntity = Assignment.Create(student, new AssignmentCreateRequestDto(
                    faker.Hacker.IngVerb(),
                    faker.Lorem.Paragraph(1),
                    null,
                    DateTime.UtcNow.AddDays(1)));
                
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