using Bogus;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class SubjectFaker : Faker<Subject>
    {
        public static readonly SubjectFaker Instance = new SubjectFaker();

        private SubjectFaker()
        {
            RuleFor(t => t.Id, f => f.IndexFaker + 1);
            RuleFor(t => t.Name, f => f.Company.CompanyName());
        }
    }
}