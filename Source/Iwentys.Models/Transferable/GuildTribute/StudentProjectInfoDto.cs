using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.GuildTribute
{
    public class StudentProjectInfoDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static StudentProjectInfoDto Wrap(StudentProject project)
        {
            return new StudentProjectInfoDto
            {
                Id = project.Id,
                Url = project.FullUrl,
                Name = project.FullUrl,
                Description = project.Description
            };
        }
    }
}