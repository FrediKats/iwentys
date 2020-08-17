using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Gamification;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IQuestRepository : IGenericRepository<Quest, int>
    {
        void AcceptQuest(Quest quest, int userId);
        void SetCompleted(Quest quest, int studentId);

        Quest Create(Student student, CreateQuestDto createQuest);
    }
}