using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IStudyGroupRepository : IGenericRepository<StudyGroupEntity, int>
    {
        Task<StudyGroupEntity> ReadByNamePattern(GroupName group);
        Task<StudyGroupEntity> Create(StudyGroupEntity entity);
    }
}