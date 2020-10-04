using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IQuestRepository : IGenericRepository<QuestEntity, int>
    {
        void SendResponse(QuestEntity questEntity, int userId);
        QuestEntity SetCompleted(QuestEntity questEntity, int studentId);

        QuestEntity Create(StudentEntity student, CreateQuestRequest createQuest);
    }
}