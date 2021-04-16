using Bogus;
using Iwentys.Domain.Study;

namespace Iwentys.Database.Seeding.FakerEntities.Study
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