using System.Linq;
using Iwentys.Core.Gamification;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.IsuIntegrator;
using Iwentys.IsuIntegrator.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Core.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly DatabaseAccessor _databaseAccessor;

        private readonly AchievementProvider _achievementProvider;
        private readonly IIsuAccessor _isuAccessor;

        public StudentService(DatabaseAccessor databaseAccessor, IIsuAccessor isuAccessor, AchievementProvider achievementProvider)
        {
            _isuAccessor = isuAccessor;
            _achievementProvider = achievementProvider;
            _databaseAccessor = databaseAccessor;
        }

        public StudentFullProfileDto[] Get()
        {
            return _databaseAccessor.Student.Read().Select(s => new StudentFullProfileDto(s)).ToArray();
        }

        public StudentFullProfileDto Get(int id)
        {
            return _databaseAccessor.Student.Get(id).To(s => new StudentFullProfileDto(s));
        }

        public StudentFullProfileDto GetOrCreate(int id)
        {
            StudentEntity student = _databaseAccessor.Student.ReadById(id);
            if (student != null)
                return student.To(s => new StudentFullProfileDto(s));

            IsuUser userInfo = _isuAccessor.GetIsuUser(id, null);
            student = _databaseAccessor.Student.Create(StudentEntity.CreateFromIsu(userInfo.Id, userInfo.FirstName, userInfo.MiddleName, userInfo.SecondName));

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

        public StudentFullProfileDto RemoveGithubUsername(int id, string githubUsername)
        {
            StudentEntity user = _databaseAccessor.Student.Get(id);
            user.GithubUsername = null;
            return _databaseAccessor.Student.Update(user).To(s => new StudentFullProfileDto(s));
        }
    }
}