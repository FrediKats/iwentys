using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models.Students;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly AchievementProvider _achievementProvider;

        public StudentService(IUnitOfWork unitOfWork, AchievementProvider achievementProvider)
        {
            _unitOfWork = unitOfWork;
            _achievementProvider = achievementProvider;
            _studentRepository = _unitOfWork.GetRepository<Student>();
        }

        public async Task<List<StudentInfoDto>> Get()
        {
            List<Student> students = await _studentRepository
                .Get()
                .ToListAsync();

            return students.SelectToList(s => new StudentInfoDto(s));
        }

        public async Task<StudentInfoDto> Get(int id)
        {
            Student student = await _studentRepository.FindByIdAsync(id);
            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> GetOrCreate(int id)
        {
            Student student = await _studentRepository.FindByIdAsync(id);
            if (student is null)
            {
                var newStudent = Student.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName");
                await _studentRepository.InsertAsync(newStudent);
                student = await _studentRepository.FindByIdAsync(newStudent.Id);
            }

            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> AddGithubUsername(int id, string githubUsername)
        {
            bool isUsernameUsed = await _studentRepository.Get().AnyAsync(s => s.GithubUsername == githubUsername);
            if (isUsernameUsed)
                throw InnerLogicException.StudentExceptions.GithubAlreadyUser(githubUsername);

            //TODO: implement github access validation
            //throw new NotImplementedException("Need to validate github credentials");
            Student user = await _studentRepository.FindByIdAsync(id);
            user.GithubUsername = githubUsername;
            _studentRepository.Update(user);

            await _achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            await _unitOfWork.CommitAsync();

            return new StudentInfoDto(await _studentRepository.FindByIdAsync(id));
        }

        public async Task<StudentInfoDto> RemoveGithubUsername(int id, string githubUsername)
        {
            Student user = await _studentRepository.FindByIdAsync(id);
            user.GithubUsername = githubUsername;
            _studentRepository.Update(user);
            return new StudentInfoDto(await _studentRepository.FindByIdAsync(id));
        }
    }
}