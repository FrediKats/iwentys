using System.Linq;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Repositories
{
    public interface ISubjectRepository
    {
        public IQueryable<SubjectEntity> Read();
    }
}