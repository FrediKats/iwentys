using System.Linq;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface ISubjectRepository
    {
        public IQueryable<SubjectEntity> Read();
    }
}