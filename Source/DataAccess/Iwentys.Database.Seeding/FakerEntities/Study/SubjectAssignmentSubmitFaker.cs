using System;
using Bogus;
using Iwentys.Features.Study.SubjectAssignments.Entities;

namespace Iwentys.Database.Seeding.FakerEntities.Study
{
    public class SubjectAssignmentSubmitFaker
    {
        public static readonly SubjectAssignmentSubmitFaker Instance = new SubjectAssignmentSubmitFaker();

        private readonly Faker _faker = new Faker();

        public SubjectAssignmentSubmit Create(int subjectAssignmentId, int studentId)
        {
            return new SubjectAssignmentSubmit
            {
                Id = _faker.IndexVariable++ + 1,
                SubjectAssignmentId = subjectAssignmentId,
                StudentId = studentId,
                SubmitTimeUtc = DateTime.UtcNow
            };
        }
    }
}