using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class Teacher
    {
        public int Id { get; init; }
        public string Name { get; init; }

        public int? StudentProfileId { get; init; }
        public virtual Student StudentProfile { get; init; }
    }
}