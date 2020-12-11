using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Services
{
    public class QuestService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IQuestRepository _questRepository;

        private readonly AchievementProvider _achievementProvider;

        private readonly BarsPointTransactionLogService _pointTransactionLogService;

        public QuestService(IStudentRepository studentRepository, IQuestRepository questRepository, AchievementProvider achievementProvider, BarsPointTransactionLogService pointTransactionLogService)
        {
            _studentRepository = studentRepository;
            _questRepository = questRepository;
            _achievementProvider = achievementProvider;
            _pointTransactionLogService = pointTransactionLogService;
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
                .Where(QuestEntity.IsCompletedBy(user))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetActiveAsync()
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(QuestEntity.IsActive)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetArchivedAsync()
        {
            List<QuestEntity> quests = await _questRepository.Read()
                .Where(QuestEntity.IsArchived)
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
            var questResponseEntity = questEntity.CreateResponse(user);
            await _questRepository.SendResponseAsync(questResponseEntity);

            QuestEntity updatedQuest = await _questRepository.ReadByIdAsync(id);
            return QuestInfoResponse.Wrap(updatedQuest);
        }

        public async Task<QuestInfoResponse> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            var quest = await _questRepository.GetAsync(questId);
            var executor = await _studentRepository.GetAsync(userId);
            
            quest.MakeCompleted(author, executor);
            //TODO: fix this
            _questRepository.Quests.Update(quest);
            await _questRepository.SaveChangesAsync();
            await _pointTransactionLogService.TransferFromSystem(userId, quest.Price);

            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return new QuestInfoResponse(quest);
        }

        public async Task<QuestInfoResponse> RevokeAsync(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = await _questRepository.ReadByIdAsync(questId);
            
            questEntity.Revoke(author);
            
            QuestEntity updatedQuest = await _questRepository.UpdateAsync(questEntity);
            return QuestInfoResponse.Wrap(updatedQuest);
        }
    }
}