namespace Iwentys.Models.Entities.Study
{
    public class TeacherEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? StudentProfileId { get; set; }
        public StudentEntity StudentProfile { get; set; }
    }
}