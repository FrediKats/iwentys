using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Gamification;
using Iwentys.Database;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Types;

namespace Iwentys.Core.Services
{
    public class QuestService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly AchievementProvider _achievementProvider;

        public QuestService(DatabaseAccessor databaseAccessor, AchievementProvider achievementProvider)
        {
            _achievementProvider = achievementProvider;
            _databaseAccessor = databaseAccessor;
        }

        public List<QuestInfoResponse> GetCreatedByUser(AuthorizedUser user)
        {
            return _databaseAccessor.Quest
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .SelectToList(QuestInfoResponse.Wrap);
        }

        public List<QuestInfoResponse> GetCompletedByUser(AuthorizedUser user)
        {
            return _databaseAccessor.Quest.Read()
                .Where(q => q.State == QuestState.Completed && q.Responses.Any(r => r.StudentId == user.Id))
                .SelectToList(QuestInfoResponse.Wrap);
        }

        public List<QuestInfoResponse> GetActive()
        {
            return _databaseAccessor.Quest
                .Read()
                .Where(q => q.State == QuestState.Active && (q.Deadline == null || q.Deadline > DateTime.UtcNow))
                .SelectToList(QuestInfoResponse.Wrap);
        }

        public List<QuestInfoResponse> GetArchived()
        {
            List<QuestEntity> repos = _databaseAccessor.Quest
                .Read()
                .Where(q => q.State == QuestState.Completed || q.Deadline > DateTime.UtcNow)
                .ToList();

            return repos
                .SelectToList(QuestInfoResponse.Wrap);
        }

        public QuestInfoResponse Create(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = user.GetProfile(_databaseAccessor.Student);
            QuestInfoResponse quest = _databaseAccessor.Quest.Create(student, createQuest).To(QuestInfoResponse.Wrap);
            _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return quest;
        }

        public QuestInfoResponse SendResponse(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = _databaseAccessor.Quest.ReadById(id);
            if (questEntity.State != QuestState.Active || questEntity.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            _databaseAccessor.Quest.SendResponse(questEntity, user.Id);
            return _databaseAccessor.Quest.ReadById(id).To(QuestInfoResponse.Wrap);
        }

        public QuestInfoResponse Complete(AuthorizedUser author, int questId, int userId)
        {
            QuestEntity questEntity = _databaseAccessor.Quest.ReadById(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            QuestInfoResponse completedQuest = _databaseAccessor.Quest.SetCompleted(questEntity, userId).To(QuestInfoResponse.Wrap);
            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return completedQuest;
        }

        public async Task<QuestInfoResponse> Revoke(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = _databaseAccessor.Quest.ReadById(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            if (questEntity.State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            questEntity.State = QuestState.Revoked;
            QuestEntity updatedQuest = await _databaseAccessor.Quest.Update(questEntity);
            return QuestInfoResponse.Wrap(updatedQuest);
        }
    }
}