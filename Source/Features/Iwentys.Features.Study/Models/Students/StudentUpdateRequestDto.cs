namespace Iwentys.Features.Study.Models.Students
{
    public record StudentUpdateRequestDto
    {
        public StudentUpdateRequestDto(string githubUsername)
        {
            GithubUsername = githubUsername;
        }

        public StudentUpdateRequestDto()
        {
        }

        public string GithubUsername { get; init; }
    }
}