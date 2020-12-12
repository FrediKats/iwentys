namespace Iwentys.Features.Students.Models
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