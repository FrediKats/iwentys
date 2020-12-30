using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? StudentProfileId { get; set; }
        public virtual Student StudentProfile { get; set; }
    }
}