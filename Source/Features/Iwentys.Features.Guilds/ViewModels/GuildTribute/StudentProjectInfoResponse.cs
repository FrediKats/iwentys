using Iwentys.Features.GithubIntegration.Entities;

namespace Iwentys.Features.Guilds.ViewModels.GuildTribute
{
    public class StudentProjectInfoResponse
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static StudentProjectInfoResponse Wrap(GithubProjectEntity projectEntity)
        {
            return new StudentProjectInfoResponse
            {
                Id = projectEntity.Id,
                Url = projectEntity.FullUrl,
                Name = projectEntity.FullUrl,
                Description = projectEntity.Description
            };
        }
    }
}