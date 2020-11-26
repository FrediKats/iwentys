using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IStudyGroupRepository : IGenericRepository<StudyGroupEntity, int>
    {
        Task<StudyGroupEntity> ReadByNamePattern(GroupName group);
        Task<StudyGroupEntity> Create(StudyGroupEntity entity);
    }
}