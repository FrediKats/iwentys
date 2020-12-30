using Bogus;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class SubjectGenerator
    {
        private int _lastId = 1;

        public Faker<Subject> Faker { get; }

        public SubjectGenerator()
        {
            Faker = new Faker<Subject>()
                .RuleFor(t => t.Id, _ => _lastId++)
                .RuleFor(t => t.Name, f => f.Company.CompanyName());
        }
    }
}