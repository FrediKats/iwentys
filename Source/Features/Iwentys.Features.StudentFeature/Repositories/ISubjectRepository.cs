using System.Linq;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface ISubjectRepository
    {
        public IQueryable<SubjectEntity> Read();
    }
}