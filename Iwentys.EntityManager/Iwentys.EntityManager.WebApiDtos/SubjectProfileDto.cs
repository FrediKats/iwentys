namespace Iwentys.EntityManager.WebApiDtos;

public record SubjectProfileDto
{
    public SubjectProfileDto(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    public SubjectProfileDto()
    {
    }

    public int Id { get; set; }
    public string Name { get; set; }
}