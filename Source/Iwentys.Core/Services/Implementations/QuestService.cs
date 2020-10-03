using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Gamification;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services.Implementations
{
    public class QuestService : IQuestService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly AchievementProvider _achievementProvider;

        public QuestService(DatabaseAccessor databaseAccessor, AchievementProvider achievementProvider)
        {
            _achievementProvider = achievementProvider;
            _databaseAccessor = databaseAccessor;
        }

        public List<QuestInfoDto> GetCreatedByUser(AuthorizedUser user)
        {
            return _databaseAccessor.Quest
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetCompletedByUser(AuthorizedUser user)
        {
            return _databaseAccessor.Quest.Read()
                .Where(q => q.State == QuestState.Completed && q.Responses.Any(r => r.StudentId == user.Id))
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetActive()
        {
            return _databaseAccessor.Quest
                .Read()
                .Where(q => q.State == QuestState.Active && (q.Deadline == null || q.Deadline > DateTime.UtcNow))
                .SelectToList(QuestInfoDto.Wrap);
        }

        public List<QuestInfoDto> GetArchived()
        {
            List<QuestEntity> repos = _databaseAccessor.Quest
                .Read()
                .Where(q => q.State == QuestState.Completed || q.Deadline > DateTime.UtcNow)
                .ToList();

            return repos
                .SelectToList(QuestInfoDto.Wrap);
        }

        public QuestInfoDto Create(AuthorizedUser user, CreateQuestDto createQuest)
        {
            StudentEntity student = user.GetProfile(_databaseAccessor.Student);
            QuestInfoDto quest = _databaseAccessor.Quest.Create(student, createQuest).To(QuestInfoDto.Wrap);
            _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return quest;
        }

        public QuestInfoDto SendResponse(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = _databaseAccessor.Quest.ReadById(id);
            if (questEntity.State != QuestState.Active || questEntity.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            _databaseAccessor.Quest.SendResponse(questEntity, user.Id);
            return _databaseAccessor.Quest.ReadById(id).To(QuestInfoDto.Wrap);
        }

        public QuestInfoDto Complete(AuthorizedUser author, int questId, int userId)
        {
            QuestEntity questEntity = _databaseAccessor.Quest.ReadById(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            QuestInfoDto completedQuest = _databaseAccessor.Quest.SetCompleted(questEntity, userId).To(QuestInfoDto.Wrap);
            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return completedQuest;
        }

        public QuestInfoDto Revoke(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = _databaseAccessor.Quest.ReadById(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            if (questEntity.State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            questEntity.State = QuestState.Revoked;
            return _databaseAccessor.Quest.Update(questEntity).To(QuestInfoDto.Wrap);
        }
    }
}