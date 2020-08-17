using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IQuestRepository : IGenericRepository<Quest, int>
    {
        void AcceptQuest(Quest quest, int userId);
        void SetCompleted(Quest quest, int userId);
    }
}