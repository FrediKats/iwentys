using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Core.Gamification;
using Iwentys.Database;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Core.Services
{
    public class StudentService
    {
        private readonly DatabaseAccessor _databaseAccessor;

        private readonly AchievementProvider _achievementProvider;

        public StudentService(DatabaseAccessor databaseAccessor, AchievementProvider achievementProvider)
        {
            _achievementProvider = achievementProvider;
            _databaseAccessor = databaseAccessor;
        }

        public async Task<List<StudentFullProfileDto>> GetAsync()
        {
            List<StudentEntity> students = await _databaseAccessor.Student
                .Read()
                .ToListAsync();

            return students.SelectToList(s => new StudentFullProfileDto(s));
        }

        public async Task<StudentFullProfileDto> GetAsync(int id)
        {
            StudentEntity student = await _databaseAccessor.Student.GetAsync(id);
            return new StudentFullProfileDto(student);
        }

        public async Task<List<StudentFullProfileDto>> GetAsync(string groupName)
        {
            var group = new GroupName(groupName);
            List<StudentEntity> students = await _databaseAccessor.Student
                .Read()
                .Where(s => s.Group.GroupName == group.Name)
                .ToListAsync();

            return students.SelectToList(s => new StudentFullProfileDto(s));
        }

        public async Task<StudentFullProfileDto> GetOrCreateAsync(int id)
        {
            StudentEntity student = await _databaseAccessor.Student.ReadByIdAsync(id);
            if (student is null)
            {
                student = await _databaseAccessor.Student.CreateAsync(StudentEntity.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName"));
                student = await _databaseAccessor.Student.GetAsync(student.Id);
            }

            return new StudentFullProfileDto(student);
        }

        public async Task<StudentFullProfileDto> AddGithubUsernameAsync(int id, string githubUsername)
        {
            if (_databaseAccessor.Student.Read().Any(s => s.GithubUsername == githubUsername))
                throw InnerLogicException.StudentEx.GithubAlreadyUser(githubUsername);

            //TODO:
            //throw new NotImplementedException("Need to validate github credentials");
            StudentEntity user = await _databaseAccessor.Student.GetAsync(id);
            user.GithubUsername = githubUsername;
            await _databaseAccessor.Student.UpdateAsync(user);

            _achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            user = await _databaseAccessor.Student.GetAsync(id);
            return new StudentFullProfileDto(user);
        }

        public async Task<StudentFullProfileDto> RemoveGithubUsernameAsync(int id, string githubUsername)
        {
            StudentEntity user = await _databaseAccessor.Student.GetAsync(id);
            user.GithubUsername = null;
            StudentEntity updatedUser = await _databaseAccessor.Student.UpdateAsync(user);
            return new StudentFullProfileDto(updatedUser);
        }
    }
}