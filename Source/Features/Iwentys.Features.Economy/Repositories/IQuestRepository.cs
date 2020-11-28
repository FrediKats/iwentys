using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;

namespace Iwentys.Features.Economy.Repositories
{
    public interface IQuestRepository : IGenericRepository<QuestEntity, int>
    {
        Task SendResponseAsync(QuestEntity questEntity, int userId);
        Task<QuestEntity> SetCompletedAsync(QuestEntity questEntity, int studentId);
        Task<QuestEntity> CreateAsync(StudentEntity student, CreateQuestRequest createQuest);
    }
}