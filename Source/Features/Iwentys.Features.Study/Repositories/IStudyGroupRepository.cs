using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Repositories
{
    public interface IStudyGroupRepository : IGenericRepository<StudyGroupEntity, int>
    {
        Task<StudyGroupEntity> ReadByNamePattern(GroupName group);
        Task<StudyGroupEntity> Create(StudyGroupEntity entity);
    }
}