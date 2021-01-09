using Bogus;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Study.SubjectAssignments.Entities;

namespace Iwentys.Database.Seeding.FakerEntities.Study
{
    public class SubjectAssignmentFaker
    {
        public static readonly SubjectAssignmentFaker Instance = new SubjectAssignmentFaker();

        private readonly Faker _faker = new Faker();

        public SubjectAssignment Create(int subjectId, Assignment assignment)
        {
            return new SubjectAssignment
            {
                Id = _faker.IndexVariable++ + 1,
                AssignmentId = assignment.Id,
                SubjectId = subjectId,
            };
        }
    }
}