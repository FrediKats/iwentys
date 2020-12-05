using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record SubjectProfileDto(int Id, string Name)
    {
        public SubjectProfileDto(SubjectEntity entity) : this(entity.Id, entity.Name)
        {
        }
    }
}