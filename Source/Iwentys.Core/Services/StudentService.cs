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

        public StudentFullProfileDto[] Get()
        {
            return _databaseAccessor.Student
                .Read()
                .AsEnumerable()
                .Select(s => new StudentFullProfileDto(s)).ToArray();
        }

        public StudentFullProfileDto Get(int id)
        {
            return _databaseAccessor.Student.Get(id).To(s => new StudentFullProfileDto(s));
        }

        public List<StudentFullProfileDto> Get(string groupName)
        {
            var group = new GroupName(groupName);

            return _databaseAccessor.Student
                .Read()
                .Where(s => s.Group.GroupName == group.Name)
                .AsEnumerable()
                .SelectToList(s => new StudentFullProfileDto(s));
        }

        public StudentFullProfileDto GetOrCreate(int id)
        {
            StudentEntity student = _databaseAccessor.Student.ReadById(id);
            if (student != null)
                return student.To(s => new StudentFullProfileDto(s));

            student = _databaseAccessor.Student.Create(StudentEntity.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName"));

            student = _databaseAccessor.Student.Get(student.Id);

            return student
                .To(s => new StudentFullProfileDto(s));
        }

        public StudentFullProfileDto AddGithubUsername(int id, string githubUsername)
        {
            if (_databaseAccessor.Student.Read().Any(s => s.GithubUsername == githubUsername))
                throw InnerLogicException.StudentEx.GithubAlreadyUser(githubUsername);

            //TODO:
            //throw new NotImplementedException("Need to validate github credentials");
            StudentEntity user = _databaseAccessor.Student.Get(id);
            user.GithubUsername = githubUsername;
            _databaseAccessor.Student.Update(user);

            _achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            return new StudentFullProfileDto(_databaseAccessor.Student.Get(id));
        }

        public async Task<StudentFullProfileDto> RemoveGithubUsername(int id, string githubUsername)
        {
            StudentEntity user = _databaseAccessor.Student.Get(id);
            user.GithubUsername = null;
            StudentEntity updatedUser = await _databaseAccessor.Student.Update(user);
            return new StudentFullProfileDto(updatedUser);
        }
    }
}