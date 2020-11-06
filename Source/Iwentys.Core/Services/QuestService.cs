using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.DomainModel;
using Iwentys.Database.Context;
using Iwentys.Features.Achievements;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<QuestInfoResponse>> GetCreatedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> entities = await _databaseAccessor.Quest
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .ToListAsync();

            return entities.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetCompletedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> quests = await _databaseAccessor.Quest.Read()
                .Where(q => q.State == QuestState.Completed && q.Responses.Any(r => r.StudentId == user.Id))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetActiveAsync()
        {
            List<QuestEntity> quests = await _databaseAccessor.Quest.Read()
                .Where(q => q.State == QuestState.Active && (q.Deadline == null || q.Deadline > DateTime.UtcNow))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetArchivedAsync()
        {
            List<QuestEntity> quests = await _databaseAccessor.Quest.Read()
                .Where(q => q.State == QuestState.Completed || q.Deadline > DateTime.UtcNow)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<QuestInfoResponse> CreateAsync(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = await user.GetProfile(_databaseAccessor.Student);
            QuestEntity quest = await _databaseAccessor.Quest.CreateAsync(student, createQuest);
            _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return QuestInfoResponse.Wrap(quest);
        }

        public async Task<QuestInfoResponse> SendResponseAsync(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = await _databaseAccessor.Quest.ReadByIdAsync(id);
            if (questEntity.State != QuestState.Active || questEntity.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            _databaseAccessor.Quest.SendResponse(questEntity, user.Id);
            QuestEntity updatedQuest = await _databaseAccessor.Quest.ReadByIdAsync(id);
            return QuestInfoResponse.Wrap(updatedQuest);
        }

        public async Task<QuestInfoResponse> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            QuestEntity questEntity = await _databaseAccessor.Quest.ReadByIdAsync(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            QuestInfoResponse completedQuest = _databaseAccessor.Quest.SetCompleted(questEntity, userId).To(QuestInfoResponse.Wrap);
            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return completedQuest;
        }

        public async Task<QuestInfoResponse> Revoke(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = await _databaseAccessor.Quest.ReadByIdAsync(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            if (questEntity.State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            questEntity.State = QuestState.Revoked;
            QuestEntity updatedQuest = await _databaseAccessor.Quest.UpdateAsync(questEntity);
            return QuestInfoResponse.Wrap(updatedQuest);
        }
    }
}