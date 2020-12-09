using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Repositories
{
    public interface IQuestRepository : IGenericRepository<QuestEntity, int>
    {
        DbSet<QuestEntity> Quests { get; }

        Task SendResponseAsync(QuestResponseEntity questResponse);
        Task<QuestEntity> CreateAsync(StudentEntity student, CreateQuestRequest createQuest);
    }
}