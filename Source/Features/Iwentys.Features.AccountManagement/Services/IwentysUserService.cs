using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.AccountManagement.Services
{
    public class IwentysUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<UniversitySystemUser> _universitySystemUserRepository;
        private readonly IGenericRepository<IwentysUser> _userRepository;

        public IwentysUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _universitySystemUserRepository = _unitOfWork.GetRepository<UniversitySystemUser>();
            _userRepository = _unitOfWork.GetRepository<IwentysUser>();
        }

        public Task<IwentysUserInfoDto> Get(int id)
        {
            return _userRepository
                .Get()
                .Where(u => u.Id == id)
                .Select(u => new IwentysUserInfoDto(u))
                .SingleAsync();
        }

        public async Task<UniversitySystemUserInfoDto> Create(UniversitySystemUserCreateArguments createArguments)
        {
            var iwentysUser = UniversitySystemUser.Create(createArguments);

            _universitySystemUserRepository.Insert(iwentysUser);
            await _unitOfWork.CommitAsync();

            return new UniversitySystemUserInfoDto(iwentysUser);
        }

        public async Task<IwentysUserInfoDto> Create(IwentysUserCreateArguments createArguments)
        {
            var iwentysUser = IwentysUser.Create(createArguments);

            await _userRepository.InsertAsync(iwentysUser);
            await _unitOfWork.CommitAsync();

            return new IwentysUserInfoDto(iwentysUser);
        }

        public async Task<IwentysUserInfoDto> AddGithubUsername(int id, string githubUsername)
        {
            var isUsernameUsed = await _userRepository.Get().AnyAsync(s => s.GithubUsername == githubUsername);
            if (isUsernameUsed)
                throw InnerLogicException.StudentExceptions.GithubAlreadyUser(githubUsername);

            //throw new NotImplementedException("Need to validate github credentials");
            IwentysUser user = await _userRepository.GetById(id);
            user.GithubUsername = githubUsername;
            _userRepository.Update(user);

            //await _achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            await _unitOfWork.CommitAsync();

            return new IwentysUserInfoDto(await _userRepository.FindByIdAsync(id));
        }

        public async Task<IwentysUserInfoDto> RemoveGithubUsername(int id, string githubUsername)
        {
            IwentysUser user = await _userRepository.GetById(id);
            
            user.UpdateGithubUsername(githubUsername);

            _userRepository.Update(user);
            await _unitOfWork.CommitAsync();

            return new IwentysUserInfoDto(user);
        }
    }
}