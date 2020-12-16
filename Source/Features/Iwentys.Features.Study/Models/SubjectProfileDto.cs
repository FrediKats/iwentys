using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record SubjectProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SubjectProfileDto(SubjectEntity entity) : this(entity.Id, entity.Name)
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
    }
}