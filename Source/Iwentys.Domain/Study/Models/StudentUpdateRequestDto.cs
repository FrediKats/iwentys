namespace Iwentys.Domain.Study
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