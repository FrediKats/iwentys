using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Enums;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Models;
using Iwentys.FeatureBase;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Services
{
    public class QuestService
    {
        private readonly AchievementProvider _achievementProvider;

        private readonly BarsPointTransactionLogService _pointTransactionLogService;
        private readonly IGenericRepository<Quest> _questRepository;
        private readonly IGenericRepository<QuestResponse> _questResponseRepository;

        private readonly IGenericRepository<IwentysUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public QuestService(AchievementProvider achievementProvider, BarsPointTransactionLogService pointTransactionLogService, IUnitOfWork unitOfWork)
        {
            _achievementProvider = achievementProvider;
            _pointTransactionLogService = pointTransactionLogService;
            _unitOfWork = unitOfWork;

            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
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
            IwentysUser student = await _userRepository.FindByIdAsync(user.Id);
            var quest = Quest.New(student, createQuest);

            await _questRepository.InsertAsync(quest);
            _userRepository.Update(student);

            _achievementProvider.AchieveForStudent(AchievementList.QuestCreator, user.Id);
            await AchievementHack.ProcessAchievement(_achievementProvider, _unitOfWork);
            await _unitOfWork.CommitAsync();

            return await Get(quest.Id);
        }

        public async Task<QuestInfoDto> SendResponse(AuthorizedUser user, int questId, QuestResponseCreateArguments arguments)
        {
            Quest quest = await _questRepository.FindByIdAsync(questId);

            QuestResponse questResponseEntity = quest.CreateResponse(user, arguments);

            await _questResponseRepository.InsertAsync(questResponseEntity);
            await _unitOfWork.CommitAsync();
            return await Get(questId);
        }

        public async Task<QuestInfoDto> Complete(AuthorizedUser author, int questId, QuestCompleteArguments arguments)
        {
            Quest quest = await _questRepository.GetById(questId);
            IwentysUser executor = await _userRepository.GetById(arguments.UserId);

            quest.MakeCompleted(author, executor, arguments);

            _questRepository.Update(quest);
            await _pointTransactionLogService.TransferFromSystem(executor.Id, quest.Price);

            _achievementProvider.AchieveForStudent(AchievementList.QuestComplete, executor.Id);
            await AchievementHack.ProcessAchievement(_achievementProvider, _unitOfWork);
            await _unitOfWork.CommitAsync();

            return await Get(questId);
        }

        public async Task<QuestInfoDto> Revoke(AuthorizedUser user, int questId)
        {
            IwentysUser author = await _userRepository.FindByIdAsync(user.Id);
            Quest quest = await _questRepository.FindByIdAsync(questId);

            quest.Revoke(author);

            _userRepository.Update(author);
            _questRepository.Update(quest);
            await _unitOfWork.CommitAsync();

            return await Get(questId);
        }

        public async Task<List<QuestRatingRow>> GetQuestExecutorRating()
        {
            List<QuestRatingRow> result = _questRepository
                .Get()
                .Where(q => q.State == QuestState.Completed)
                .AsEnumerable()
                .GroupBy(q => q.ExecutorId, q => q.ExecutorMark)
                .Select(g => new QuestRatingRow { UserId = g.Key.Value, Marks = g.ToList() })
                .ToList();

            List<IwentysUser> users = await _userRepository.Get().ToListAsync();
            //TODO: hack
            result.ForEach(r => { r.User = new IwentysUserInfoDto(users.First(u => u.Id == r.UserId));});

            return result;
        }
    }
}