using Bogus;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class TeacherGenerator
    {
        private int _teacherLastId = 1;

        public Faker<TeacherEntity> Faker { get; }

        public TeacherGenerator()
        {
            Faker = new Faker<TeacherEntity>()
                .RuleFor(t => t.Id, _ => _teacherLastId++)
                .RuleFor(t => t.Name, f => f.Name.FullName());
        }
    }
}