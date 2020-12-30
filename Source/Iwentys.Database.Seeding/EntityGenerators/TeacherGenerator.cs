using Bogus;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class TeacherGenerator
    {
        private int _teacherLastId = 1;

        public Faker<Teacher> Faker { get; }

        public TeacherGenerator()
        {
            Faker = new Faker<Teacher>()
                .RuleFor(t => t.Id, _ => _teacherLastId++)
                .RuleFor(t => t.Name, f => f.Name.FullName());
        }
    }
}