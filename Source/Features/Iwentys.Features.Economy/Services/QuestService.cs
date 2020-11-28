using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements;
using Iwentys.Features.Economy.Repositories;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Core.Services
{
    public class QuestService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IQuestRepository _questRepository;

        private readonly AchievementProvider _achievementProvider;

        public QuestService(IStudentRepository studentRepository, IQuestRepository questRepository, AchievementProvider achievementProvider)
        {
            _studentRepository = studentRepository;
            _questRepository = questRepository;
            _achievementProvider = achievementProvider;
        }

        public async Task<List<QuestInfoResponse>> GetCreatedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> entities = await _questRepository
                .Read()
                .Where(q => q.AuthorId == user.Id)
                .ToListAsync();

            return entities.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetCompletedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(q => q.State == QuestState.Completed && q.Responses.Any(r => r.StudentId == user.Id))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetActiveAsync()
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(q => q.State == QuestState.Active && (q.Deadline == null || q.Deadline > DateTime.UtcNow))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetArchivedAsync()
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(q => q.State == QuestState.Completed || q.Deadline > DateTime.UtcNow)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<QuestInfoResponse> CreateAsync(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = await user.GetProfile(_studentRepository);
            QuestEntity quest = await _questRepository.CreateAsync(student, createQuest);
            _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return QuestInfoResponse.Wrap(quest);
        }

        public async Task<QuestInfoResponse> SendResponseAsync(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(id);
            if (questEntity.State != QuestState.Active || questEntity.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            _questRepository.SendResponse(questEntity, user.Id);
            QuestEntity updatedQuest = await _questRepository.ReadByIdAsync(id);
            return QuestInfoResponse.Wrap(updatedQuest);
        }

        public async Task<QuestInfoResponse> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            QuestInfoResponse completedQuest = _questRepository.SetCompleted(questEntity, userId).To(QuestInfoResponse.Wrap);
            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return completedQuest;
        }

        public async Task<QuestInfoResponse> Revoke(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(questId);
            if (questEntity.AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            if (questEntity.State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            questEntity.State = QuestState.Revoked;
            QuestEntity updatedQuest = await _questRepository.UpdateAsync(questEntity);
            return QuestInfoResponse.Wrap(updatedQuest);
        }
    }
}