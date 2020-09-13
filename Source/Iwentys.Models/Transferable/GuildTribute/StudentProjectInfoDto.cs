using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.GuildTribute
{
    public class StudentProjectInfoDto
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static StudentProjectInfoDto Wrap(GithubProjectEntity projectEntity)
        {
            return new StudentProjectInfoDto
            {
                Id = projectEntity.Id,
                Url = projectEntity.FullUrl,
                Name = projectEntity.FullUrl,
                Description = projectEntity.Description
            };
        }
    }
}