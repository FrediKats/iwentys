namespace Iwentys.Features.GithubIntegration.Models
{
    public record GithubUserInfoDto
    {
        public GithubUserInfoDto(int id, string name, string avatarUrl, string bio, string company)
        {
            Id = id;
            Name = name;
            AvatarUrl = avatarUrl;
            Bio = bio;
            Company = company;
        }

        public GithubUserInfoDto()
        {
        }
        
        public int Id { get; init; }
        public string Name { get; init; }
        public string AvatarUrl { get; init; }
        public string Bio { get; init; }
        public string Company { get; init; }
    }
}