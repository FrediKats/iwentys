namespace Iwentys.Models.Entities
{
    public class StudentProject
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}