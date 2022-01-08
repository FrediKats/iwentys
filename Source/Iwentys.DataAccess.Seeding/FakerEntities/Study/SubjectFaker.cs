using Bogus;
using Iwentys.Domain.Study;

namespace Iwentys.DataAccess.Seeding;

public class SubjectFaker : Faker<Subject>
{
    public static readonly SubjectFaker Instance = new SubjectFaker();

    private SubjectFaker()
    {
        RuleFor(t => t.Id, f => f.IndexFaker + 1);
        RuleFor(t => t.Title, f => f.Company.CompanyName());
    }
}