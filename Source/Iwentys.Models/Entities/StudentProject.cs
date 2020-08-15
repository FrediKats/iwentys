namespace Iwentys.Models.Entities
{
    public class StudentProject
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }
        public long GithubRepositoryId { get; set; }
        public string UserName { get; set; }
    }
}