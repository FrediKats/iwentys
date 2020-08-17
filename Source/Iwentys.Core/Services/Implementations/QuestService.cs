using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
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

        public List<QuestInfoDto> GetCreatedByUser(AuthorizedUser user)
        {
            return _questRepository
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetCompletedByUser(AuthorizedUser user)
        {
            return _questRepository.Read()
                .Where(q => q.State == QuestState.Completed && q.Responses.Any(r => r.StudentId == user.Id))
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetActive()
        {
            return _questRepository
                .Read()
                .Where(q => q.State == QuestState.Active)
                .WhereIsNotOutdated()
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

        public QuestInfoDto Create(AuthorizedUser user, CreateQuestDto createQuest)
        {
            return _questRepository.Create(user.Profile, createQuest).To(QuestInfoDto.Wrap);
        }

        public QuestInfoDto SendResponse(AuthorizedUser user, int id)
        {
            Quest quest = _questRepository.ReadById(id);
            if (quest.State == QuestState.Completed || quest.IsOutdated())
                throw new InnerLogicException("Quest closed");

            _questRepository.AcceptQuest(quest, user.Id);
            return _questRepository.ReadById(id).To(QuestInfoDto.Wrap);
        }

        public QuestInfoDto SetCompleted(AuthorizedUser user, int questId, int userId)
        {
            Quest quest = _questRepository.ReadById(questId);
            if (quest.AuthorId != user.Id)
                throw InnerLogicException.NotEnoughPermission(user.Id);

            _questRepository.SetCompleted(quest, userId);
            return _questRepository.ReadById(questId).To(QuestInfoDto.Wrap);
        }
    }
}