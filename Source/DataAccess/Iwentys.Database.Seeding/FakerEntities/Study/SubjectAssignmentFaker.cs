using Bogus;
using Iwentys.Features.Study.SubjectAssignments.Entities;

namespace Iwentys.Database.Seeding.FakerEntities.Study
{
    public class SubjectAssignmentFaker
    {
        public static readonly SubjectAssignmentFaker Instance = new SubjectAssignmentFaker();

        private readonly Faker _faker = new Faker();

        public SubjectAssignment Create(int subjectId)
        {
            return new SubjectAssignment
            {
                Id = _faker.IndexVariable++ + 1,
                Title = _faker.Hacker.IngVerb(),
                Description = _faker.Lorem.Paragraph(),
                SubjectId = subjectId,
                Link = _faker.Internet.Url()
            };
        }
    }
}