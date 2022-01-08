using Bogus;
using Iwentys.Domain.Study;

namespace Iwentys.DataAccess.Seeding
{
    public class StudyGroupFaker
    {
        public static readonly StudyGroupFaker Instance = new StudyGroupFaker();
        private readonly Faker _faker = new Faker();

        public StudyGroup CreateGroup()
        {
            return new StudyGroup
            {
                GroupName = _faker.Lorem.Word()
            };
        }
    }
}