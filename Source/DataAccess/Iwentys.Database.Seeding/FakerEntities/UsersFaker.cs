using Bogus;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study.Enums;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class UsersFaker
    {
        public static readonly UsersFaker Instance = new UsersFaker();

        private readonly Faker _identifierFaker = new Faker();

        private UsersFaker()
        {
            UniversitySystemUsers = new Faker<UniversitySystemUserCreateArguments>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.SecondName, f => f.Name.LastName())
                .RuleFor(u => u.MiddleName, f => f.Name.Suffix());

            IwentysUsers = new Faker<IwentysUserCreateArguments>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.SecondName, f => f.Name.LastName())
                .RuleFor(u => u.MiddleName, f => f.Name.Suffix())
                .RuleFor(u => u.IsAdmin, false)
                .RuleFor(u => u.GithubUsername, f => f.Internet.UserName())
                .RuleFor(u => u.BarsPoints, 1000)
                .RuleFor(u => u.AvatarUrl, f => f.Image.PicsumUrl());

            Students = new Faker<StudentCreateArguments>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.SecondName, f => f.Name.LastName())
                .RuleFor(u => u.MiddleName, f => f.Name.Suffix())
                .RuleFor(u => u.IsAdmin, false)
                .RuleFor(u => u.GithubUsername, f => f.Internet.UserName())
                .RuleFor(u => u.BarsPoints, 1000)
                .RuleFor(u => u.AvatarUrl, f => f.Image.PicsumUrl())
                .RuleFor(u => u.Type, StudentType.Budgetary);
        }

        public Faker<UniversitySystemUserCreateArguments> UniversitySystemUsers { get; set; }
        public Faker<IwentysUserCreateArguments> IwentysUsers { get; set; }
        public Faker<StudentCreateArguments> Students { get; set; }

        public int GetIdentifier()
        {
            return _identifierFaker.IndexVariable++ + 1;
        }
    }
}