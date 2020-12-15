using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Services
{
    public class QuestService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<QuestEntity> _questRepository;
        private readonly IGenericRepository<QuestResponseEntity> _questResponseRepository;

        private readonly AchievementProvider _achievementProvider;

        private readonly BarsPointTransactionLogService _pointTransactionLogService;

        public QuestService(AchievementProvider achievementProvider, BarsPointTransactionLogService pointTransactionLogService, IUnitOfWork unitOfWork)
        {
            _achievementProvider = achievementProvider;
            _pointTransactionLogService = pointTransactionLogService;
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _questRepository = _unitOfWork.GetRepository<QuestEntity>();
            _questResponseRepository = _unitOfWork.GetRepository<QuestResponseEntity>();
        }

        public async Task<List<QuestInfoResponse>> GetCreatedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> entities = await _questRepository
                .GetAsync()
                .Where(q => q.AuthorId == user.Id)
                .ToListAsync();

            return entities.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetCompletedByUserAsync(AuthorizedUser user)
        {
            List<QuestEntity> quests = await _questRepository.GetAsync()
                .Where(QuestEntity.IsCompletedBy(user))
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetActiveAsync()
        {
            List<QuestEntity> quests = await _questRepository.GetAsync()
                .Where(QuestEntity.IsActive)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<List<QuestInfoResponse>> GetArchivedAsync()
        {
            List<QuestEntity> quests = await _questRepository.GetAsync()
                .Where(QuestEntity.IsArchived)
                .ToListAsync();

            return quests.SelectToList(QuestInfoResponse.Wrap);
        }

        public async Task<QuestInfoResponse> CreateAsync(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            var quest = QuestEntity.New(student, createQuest);
            
            await _questRepository.InsertAsync(quest);
            await _studentRepository.UpdateAsync(student);
            await _unitOfWork.CommitAsync();
            
            await _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return QuestInfoResponse.Wrap(quest);
        }

        public async Task<QuestInfoResponse> SendResponseAsync(AuthorizedUser user, int id)
        {
            QuestEntity questEntity = await _questRepository.GetByIdAsync(id);
            var questResponseEntity = questEntity.CreateResponse(user);
            await _questResponseRepository.InsertAsync(questResponseEntity);

            QuestEntity updatedQuest = await _questRepository.GetByIdAsync(id);
            return QuestInfoResponse.Wrap(updatedQuest);
        }

        public async Task<QuestInfoResponse> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            var quest = await _questRepository.GetByIdAsync(questId);
            var executor = await _studentRepository.GetByIdAsync(userId);

            quest.MakeCompleted(author, executor);
            
            await _questRepository.UpdateAsync(quest);
            await _unitOfWork.CommitAsync();
            
            await _pointTransactionLogService.TransferFromSystem(userId, quest.Price);

            await _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return new QuestInfoResponse(quest);
        }

        public async Task<QuestInfoResponse> RevokeAsync(AuthorizedUser author, int questId)
        {
            QuestEntity questEntity = await _questRepository.GetByIdAsync(questId);
            
            questEntity.Revoke(author);
            
            await _questRepository.UpdateAsync(questEntity);
            return QuestInfoResponse.Wrap(await _questRepository.GetByIdAsync(questId));
        }
    }
}