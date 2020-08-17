using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Types;
using MoreLinq;

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

        public List<QuestInfoDto> GetCreatedByUser(AuthorizedUser user)
        {
            return _questRepository
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetCompletedByUser(AuthorizedUser user)
        {
            throw new System.NotImplementedException();
        }

        public List<QuestInfoDto> GetActive()
        {
            return _questRepository
                .Read()
                .Where(q => q.State == QuestState.Active || q.State == QuestState.Accepted)
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetArchive()
        {
            List<Quest> repos = _questRepository
                .Read()
                .Where(q => q.State == QuestState.Completed || q.Deadline > DateTime.UtcNow)
                .ToList();

            repos
                .Where(q => q.State != QuestState.Completed && q.Deadline > DateTime.UtcNow)
                .ForEach(q => q.State = QuestState.Outdated);
            
            return repos
                .SelectToList(QuestInfoDto.Wrap);
        }
    }
}