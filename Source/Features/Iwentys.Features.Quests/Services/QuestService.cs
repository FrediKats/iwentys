using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Services
{
    public class QuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<QuestEntity> _questRepositoryGeneric;
        private readonly IGenericRepository<QuestResponseEntity> _questResponseRepository;

        private readonly IQuestRepository _questRepository;

        private readonly AchievementProvider _achievementProvider;

        private readonly BarsPointTransactionLogService _pointTransactionLogService;

        public QuestService(IQuestRepository questRepository, AchievementProvider achievementProvider, BarsPointTransactionLogService pointTransactionLogService, IUnitOfWork unitOfWork)
        {
            _questRepository = questRepository;
            _achievementProvider = achievementProvider;
            _pointTransactionLogService = pointTransactionLogService;
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _questRepositoryGeneric = _unitOfWork.GetRepository<QuestEntity>();
            _questResponseRepository = _unitOfWork.GetRepository<QuestResponseEntity>();
        }

        public async Task<List<QuestInfoResponse>> GetCreatedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> entities = await _questRepositoryGeneric
                .GetAsync()
                .Where(q => q.AuthorId == user.Id)
                .ToListAsync();

            return entities.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetCompletedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> quests = await _questRepositoryGeneric.GetAsync()
                .Where(QuestEntity.IsCompletedBy(user))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetActiveAsync()
        {
            List<QuestEntity> quests = await _questRepositoryGeneric.GetAsync()
                .Where(QuestEntity.IsActive)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetArchivedAsync()
        {
            List<QuestEntity> quests = await _questRepositoryGeneric.GetAsync()
                .Where(QuestEntity.IsArchived)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<QuestInfoResponse> CreateAsync(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            //TODO: move to domain
            QuestEntity quest = await _questRepository.CreateAsync(student, createQuest);
            _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return QuestInfoResponse.Wrap(quest);
        }

        public async Task<QuestInfoResponse> SendResponseAsync(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = await _questRepositoryGeneric.GetByIdAsync(id);
            var questResponseEntity = questEntity.CreateResponse(user);
            await _questResponseRepository.InsertAsync(questResponseEntity);

            QuestEntity updatedQuest = await _questRepositoryGeneric.GetByIdAsync(id);
            return QuestInfoResponse.Wrap(updatedQuest);
        }

        public async Task<QuestInfoResponse> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            var quest = await _questRepositoryGeneric.GetByIdAsync(questId);
            var executor = await _studentRepository.GetByIdAsync(userId);

            quest.MakeCompleted(author, executor);
            
            await _questRepositoryGeneric.UpdateAsync(quest);
            await _unitOfWork.CommitAsync();
            
            await _pointTransactionLogService.TransferFromSystem(userId, quest.Price);

            _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return new QuestInfoResponse(quest);
        }

        public async Task<QuestInfoResponse> RevokeAsync(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = await _questRepositoryGeneric.GetByIdAsync(questId);
            
            questEntity.Revoke(author);
            
            await _questRepositoryGeneric.UpdateAsync(questEntity);
            return QuestInfoResponse.Wrap(await _questRepositoryGeneric.GetByIdAsync(questId));
        }
    }
}