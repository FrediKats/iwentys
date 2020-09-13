using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Gamification;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IQuestRepository : IGenericRepository<Quest, int>
    {
        void SendResponse(Quest quest, int userId);
        Quest SetCompleted(Quest quest, int studentId);

        Quest Create(StudentEntity student, CreateQuestDto createQuest);
    }
}