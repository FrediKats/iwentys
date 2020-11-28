using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;

namespace Iwentys.Features.Economy.Repositories
{
    public interface IQuestRepository : IGenericRepository<QuestEntity, int>
    {
        IQueryable<QuestEntity> Read();
        Task<QuestEntity> ReadByIdAsync(int key);
        Task<QuestEntity> UpdateAsync(QuestEntity entity);
        Task<int> DeleteAsync(int key);
        void SendResponse(QuestEntity questEntity, int userId);
        QuestEntity SetCompleted(QuestEntity questEntity, int studentId);
        Task<QuestEntity> CreateAsync(StudentEntity student, CreateQuestRequest createQuest);
    }
}