using Iwentys.Domain.Study;

namespace Iwentys.Study
{
    public record SubjectProfileDto
    {
        public SubjectProfileDto(Subject entity) : this(entity.Id, entity.Title)
        {
        }

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
}