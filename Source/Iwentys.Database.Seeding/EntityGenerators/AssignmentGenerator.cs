using System;
using System.Collections.Generic;
using Bogus;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class AssignmentGenerator
    {
        public List<AssignmentEntity> Assignments { get; set; }
        public List<StudentAssignmentEntity> StudentAssignments { get; set; }

        public AssignmentGenerator(List<StudentEntity> students)
        {
            var faker = new Faker();

            Assignments = new List<AssignmentEntity>();
            StudentAssignments = new List<StudentAssignmentEntity>();
            foreach (StudentEntity student in students)
            {
                var assignmentEntity = AssignmentEntity.Create(student, new AssignmentCreateRequestDto(
                    faker.Hacker.IngVerb(),
                    faker.Lorem.Paragraph(1),
                    null,
                    DateTime.UtcNow.AddDays(1)));
                
                assignmentEntity.Id = 1 + faker.IndexVariable++;
                Assignments.Add(assignmentEntity);
                StudentAssignments.Add(new StudentAssignmentEntity
                {
                    AssignmentId = assignmentEntity.Id,
                    StudentId = student.Id,
                });
            }
        }
    }
}