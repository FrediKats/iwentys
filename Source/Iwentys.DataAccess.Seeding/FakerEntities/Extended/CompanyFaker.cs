using Bogus;
using Iwentys.Domain.Companies;

namespace Iwentys.DataAccess.Seeding;

public class CompanyFaker
{
    public static readonly CompanyFaker Instance = new CompanyFaker();

    private readonly Faker _faker = new Faker();

    public CompanyCreateArguments NewCompany()
    {
        return new CompanyCreateArguments
        {
            Name = _faker.Lorem.Word()
        };
    }
}