using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Quests.Services
{
    public class QuestService
    {
        private readonly AchievementProvider _achievementProvider;

        private readonly BarsPointTransactionLogService _pointTransactionLogService;
        private readonly IGenericRepository<Quest> _questRepository;
        private readonly IGenericRepository<QuestResponse> _questResponseRepository;

        private readonly IGenericRepository<IwentysUser> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuestService(AchievementProvider achievementProvider, BarsPointTransactionLogService pointTransactionLogService, IUnitOfWork unitOfWork)
        {
            _achievementProvider = achievementProvider;
            _pointTransactionLogService = pointTransactionLogService;
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            _questRepository = _unitOfWork.GetRepository<Quest>();
            _questResponseRepository = _unitOfWork.GetRepository<QuestResponse>();
        }

        public Task<QuestInfoDto> Get(int questId)
        {
            return _questRepository
                .Get()
                .Where(q => q.Id == questId)
                .Select(QuestInfoDto.FromEntity)
                .FirstAsync();
        }

        public async Task<List<QuestInfoDto>> GetCreatedByUser(AuthorizedUser user)
        {
            return await _questRepository
                .Get()
                .Where(q => q.AuthorId == user.Id)
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<QuestInfoDto>> GetCompletedByUser(AuthorizedUser user)
        {
            return await _questRepository.Get()
                .Where(Quest.IsCompletedBy(user))
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<QuestInfoDto>> GetActive()
        {
            return await _questRepository
                .Get()
                .Where(Quest.IsActive)
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<List<QuestInfoDto>> GetArchived()
        {
            return await _questRepository
                .Get()
                .Where(Quest.IsArchived)
                .Select(QuestInfoDto.FromEntity)
                .ToListAsync();
        }

        public async Task<QuestInfoDto> Create(AuthorizedUser user, CreateQuestRequest createQuest)
        {
            IwentysUser student = await _studentRepository.FindByIdAsync(user.Id);
            var quest = Quest.New(student, createQuest);

            await _questRepository.InsertAsync(quest);
            _studentRepository.Update(student);

            await _achievementProvider.Achieve(AchievementList.QuestCreator, user.Id);
            await _unitOfWork.CommitAsync();

            return await Get(quest.Id);
        }

        public async Task<QuestInfoDto> SendResponse(AuthorizedUser user, int questId)
        {
            Quest quest = await _questRepository.FindByIdAsync(questId);

            QuestResponse questResponseEntity = quest.CreateResponse(user);

            await _questResponseRepository.InsertAsync(questResponseEntity);
            await _unitOfWork.CommitAsync();
            return await Get(questId);
        }

        public async Task<QuestInfoDto> Complete(AuthorizedUser author, int questId, int userId)
        {
            Quest quest = await _questRepository.FindByIdAsync(questId);
            IwentysUser executor = await _studentRepository.FindByIdAsync(userId);

            quest.MakeCompleted(author, executor);

            _questRepository.Update(quest);
            await _pointTransactionLogService.TransferFromSystem(userId, quest.Price);
            await _achievementProvider.Achieve(AchievementList.QuestComplete, userId);
            await _unitOfWork.CommitAsync();

            return await Get(questId);
        }

        public async Task<QuestInfoDto> Revoke(AuthorizedUser user, int questId)
        {
            IwentysUser author = await _studentRepository.FindByIdAsync(user.Id);
            Quest quest = await _questRepository.FindByIdAsync(questId);

            quest.Revoke(author);

            _studentRepository.Update(author);
            _questRepository.Update(quest);
            await _unitOfWork.CommitAsync();

            return await Get(questId);
        }
    }
}