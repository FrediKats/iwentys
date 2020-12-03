using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.ViewModels;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Quests.Repositories
{
    public interface IQuestRepository : IGenericRepository<QuestEntity, int>
    {
        Task SendResponseAsync(QuestEntity questEntity, int userId);
        Task<QuestEntity> SetCompletedAsync(QuestEntity questEntity, int studentId);
        Task<QuestEntity> CreateAsync(StudentEntity student, CreateQuestRequest createQuest);
    }
}