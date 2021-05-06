using Iwentys.Domain.GithubIntegration;

namespace Iwentys.Domain.Study.Models
{
    public class StudentProjectInfoResponse
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorUsername { get; set; }

        public static StudentProjectInfoResponse Wrap(GithubProject project)
        {
            return new StudentProjectInfoResponse
            {
                Id = project.Id,
                Url = project.FullUrl,
                Name = project.Name,
                Description = project.Description,
                AuthorUsername = project.Owner
            };
        }
    }
}