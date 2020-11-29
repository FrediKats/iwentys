using Iwentys.Models.Entities.Study;

namespace Iwentys.Models.Transferable.Study
{
    public class SubjectProfileResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static SubjectProfileResponse Wrap(SubjectEntity entity)
        {
            return new SubjectProfileResponse
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}