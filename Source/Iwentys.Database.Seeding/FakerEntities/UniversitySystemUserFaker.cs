using Bogus;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class UniversitySystemUserFaker : Faker<UniversitySystemUser>
    {
        public static readonly UniversitySystemUserFaker Instance = new UniversitySystemUserFaker();

        private UniversitySystemUserFaker()
        {
            //TODO: remove this hack. We need shared id counter for University user and student generators
            RuleFor(t => t.Id, f => f.IndexFaker + 1000000);
            RuleFor(t => t.FirstName, f => f.Name.FirstName());
            RuleFor(t => t.SecondName, f => f.Name.LastName());
        }
    }
}