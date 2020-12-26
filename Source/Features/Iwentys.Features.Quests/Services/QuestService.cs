using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
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

        public async Task<QuestInfoDto> Get(int questId)
        {
            QuestEntity entities = await _questRepository
                .GetByIdAsync(questId);

            return new QuestInfoDto(entities);
        }

        public async Task<List<QuestInfoDto>> GetCreatedByUserAsync(AuthorizedUser user)
        {
            return await _questRepository
                .GetAsync()
                .Where(q => q.AuthorId == user.Id)
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<QuestInfoDto>> GetCompletedByUserAsync(AuthorizedUser user)
        {
            return await _questRepository.GetAsync()
                .Where(QuestEntity.IsCompletedBy(user))
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<QuestInfoDto>> GetActiveAsync()
        {
            return await _questRepository
                .GetAsync()
                .Where(QuestEntity.IsActive)
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<QuestInfoDto>> GetArchivedAsync()
        {
            return await _questRepository
                .GetAsync()
                .Where(QuestEntity.IsArchived)
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<QuestInfoDto> CreateAsync(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            StudentEntity student = await _studentRepository.GetByIdAsync(user.Id);
            var quest = QuestEntity.New(student, createQuest);
            
            await _questRepository.InsertAsync(quest);
            _studentRepository.Update(student);
            await _unitOfWork.CommitAsync();
            
            await _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            return new QuestInfoDto(quest);
        }

        public async Task<QuestInfoDto> SendResponseAsync(AuthorizedUser user, int questId)
        {
            QuestEntity questEntity = await _questRepository.GetByIdAsync(questId);
            
            var questResponseEntity = questEntity.CreateResponse(user);
            
            await _questResponseRepository.InsertAsync(questResponseEntity);
            await _unitOfWork.CommitAsync();
            QuestEntity updatedQuest = await _questRepository.GetByIdAsync(questId);
            return new QuestInfoDto(updatedQuest);
        }

        public async Task<QuestInfoDto> CompleteAsync(AuthorizedUser author, int questId, int userId)
        {
            var quest = await _questRepository.GetByIdAsync(questId);
            var executor = await _studentRepository.GetByIdAsync(userId);

            quest.MakeCompleted(author, executor);
            
            _questRepository.Update(quest);
            await _unitOfWork.CommitAsync();
            
            await _pointTransactionLogService.TransferFromSystem(userId, quest.Price);

            await _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            return new QuestInfoDto(quest);
        }

        public async Task<QuestInfoDto> RevokeAsync(AuthorizedUser user, int questId)
        {
            var author = await _studentRepository.GetByIdAsync(user.Id);
            QuestEntity quest = await _questRepository.GetByIdAsync(questId);
            
            quest.Revoke(author);
            
            _studentRepository.Update(author);
            _questRepository.Update(quest);
            await _unitOfWork.CommitAsync();

            return new QuestInfoDto(await _questRepository.GetByIdAsync(questId));
        }
    }
}