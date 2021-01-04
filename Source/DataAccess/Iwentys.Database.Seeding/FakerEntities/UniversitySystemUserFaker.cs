using Bogus;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class UniversitySystemUserFaker : Faker<UniversitySystemUser>
    {
        public static readonly UniversitySystemUserFaker Instance = new UniversitySystemUserFaker();

        private readonly Faker _identifierFaker = new Faker();

        private UniversitySystemUserFaker()
        {
            RuleFor(t => t.Id, _ => GetIdentifier());
            RuleFor(t => t.FirstName, f => f.Name.FirstName());
            RuleFor(t => t.SecondName, f => f.Name.LastName());
        }

        public int GetIdentifier()
        {
            return _identifierFaker.IndexVariable++ + 1;
        }
    }
}