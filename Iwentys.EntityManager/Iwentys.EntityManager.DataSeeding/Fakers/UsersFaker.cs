using Bogus;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.DataSeeding;

public class UsersFaker
{
    public static readonly UsersFaker Instance = new UsersFaker();

    private readonly Faker _identifierFaker = new Faker();

    private UsersFaker()
    {
        UniversitySystemUsers = new Faker<UniversitySystemUser>()
            .RuleFor(u => u.Id, GetIdentifier)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.SecondName, f => f.Name.LastName())
            .RuleFor(u => u.MiddleName, f => f.Name.Suffix());

        IwentysUsers = new Faker<IwentysUser>()
            .RuleFor(u => u.Id, GetIdentifier)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.SecondName, f => f.Name.LastName())
            .RuleFor(u => u.MiddleName, f => f.Name.Suffix())
            .RuleFor(u => u.IsAdmin, false)
            .RuleFor(u => u.GithubUsername, f => f.Internet.UserName())
            .RuleFor(u => u.AvatarUrl, f => f.Image.PicsumUrl());

        Students = new Faker<Student>()
            .RuleFor(u => u.Id, GetIdentifier)
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.SecondName, f => f.Name.LastName())
            .RuleFor(u => u.MiddleName, f => f.Name.Suffix())
            .RuleFor(u => u.IsAdmin, false)
            .RuleFor(u => u.GithubUsername, f => f.Internet.UserName())
            .RuleFor(u => u.AvatarUrl, f => f.Image.PicsumUrl())
            .RuleFor(u => u.Type, StudentType.Budgetary);
    }

    public Faker<UniversitySystemUser> UniversitySystemUsers { get; set; }
    public Faker<IwentysUser> IwentysUsers { get; set; }
    public Faker<Student> Students { get; set; }

    public int GetIdentifier()
    {
        return _identifierFaker.IndexVariable++ + 1;
    }
}