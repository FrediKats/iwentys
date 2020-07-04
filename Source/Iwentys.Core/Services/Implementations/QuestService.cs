using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Implementations
{
    public class QuestService : IQuestService
    {
        private readonly IQuestRepository _questRepository;

        public QuestService(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }


        public Quest[] Get()
        {
            return _questRepository.Read().ToArray();
        }
    }
}