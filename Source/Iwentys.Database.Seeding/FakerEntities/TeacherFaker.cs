using Bogus;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.FakerEntities
{
    public class TeacherFaker : Faker<Teacher>
    {
        public static readonly TeacherFaker Instance = new TeacherFaker();

        private TeacherFaker()
        {
            RuleFor(t => t.Id, f => f.IndexFaker + 1);
            RuleFor(t => t.Name, f => f.Name.FullName());
        }
    }
}