namespace Iwentys.Domain.Extended.Models
{
    public record CreateProjectRequestDto
    {
        public CreateProjectRequestDto(string owner, string repositoryName)
        {
            Owner = owner;
            RepositoryName = repositoryName;
        }

        public CreateProjectRequestDto()
        {
        }

        public string Owner { get; init; }
        public string RepositoryName { get; init; }
    }
}