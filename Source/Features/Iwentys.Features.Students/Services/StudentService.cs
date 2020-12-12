using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Students.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Students.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepository;
        //private readonly AchievementProvider _achievementProvider;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<List<StudentInfoDto>> GetAsync()
        {
            List<StudentEntity> students = await _studentRepository
                .Read()
                .ToListAsync();

            return students.SelectToList(s => new StudentInfoDto(s));
        }

        public async Task<StudentInfoDto> GetAsync(int id)
        {
            StudentEntity student = await _studentRepository.GetAsync(id);
            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> GetOrCreateAsync(int id)
        {
            StudentEntity student = await _studentRepository.ReadByIdAsync(id);
            if (student is null)
            {
                student = await _studentRepository.CreateAsync(StudentEntity.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName"));
                student = await _studentRepository.GetAsync(student.Id);
            }

            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> AddGithubUsernameAsync(int id, string githubUsername)
        {
            bool isUsernameUsed = await _studentRepository.Read().AnyAsync(s => s.GithubUsername == githubUsername);
            if (isUsernameUsed)
                throw InnerLogicException.Student.GithubAlreadyUser(githubUsername);

            //TODO: implement gitgub access validation
            //throw new NotImplementedException("Need to validate github credentials");
            await _studentRepository.UpdateGithub(id, githubUsername);

            //TODO: implement getting achievements for adding github
            //_achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            //TODO: ensure we need to return this
            return new StudentInfoDto(await _studentRepository.GetAsync(id));
        }

        public async Task<StudentInfoDto> RemoveGithubUsernameAsync(int id, string githubUsername)
        {
            await _studentRepository.UpdateGithub(id, githubUsername);
            return new StudentInfoDto(await _studentRepository.GetAsync(id));
        }
    }
}