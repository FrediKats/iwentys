using System;
using System.Linq;
using Iwentys.Core.Gamification;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
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
        private readonly AchievementProvider _achievementProvider;
        private readonly IStudentRepository _studentRepository;
        private readonly IIsuAccessor _isuAccessor;


        public StudentService(IStudentRepository studentRepository, IIsuAccessor isuAccessor, AchievementProvider achievementProvider)
        {
            _studentRepository = studentRepository;
            _isuAccessor = isuAccessor;
            _achievementProvider = achievementProvider;
        }

        public StudentFullProfileDto[] Get()
        {
            return _studentRepository.Read().Select(s => new StudentFullProfileDto(s)).ToArray();
        }

        public StudentFullProfileDto Get(int id)
        {
            return _studentRepository.Get(id).To(s => new StudentFullProfileDto(s));
        }

        public StudentFullProfileDto GetOrCreate(int id)
        {
            Student student = _studentRepository.ReadById(id);
            if (student != null)
                return student.To(s => new StudentFullProfileDto(s));

            IsuUser userInfo = _isuAccessor.GetIsuUser(id, null);
            student = _studentRepository.Create(Student.CreateFromIsu(userInfo.Id, userInfo.FirstName, userInfo.MiddleName, userInfo.SecondName, String.Empty));

            return student
                .To(s => new StudentFullProfileDto(s));
        }

        public StudentFullProfileDto AddGithubUsername(int id, string githubUsername)
        {
            if (_studentRepository.Read().Any(s => s.GithubUsername == githubUsername))
                throw new InnerLogicException("Username already used for other account");

            //TODO:
            //throw new NotImplementedException("Need to validate github credentials");
            Student user = _studentRepository.Get(id);
            user.GithubUsername = githubUsername;
            _studentRepository.Update(user);

            _achievementProvider.Achieve(AchievementList.AddGithubAchievement, user.Id);
            //TODO:
            return new StudentFullProfileDto(_studentRepository.Get(id));
        }

        public StudentFullProfileDto RemoveGithubUsername(int id, string githubUsername)
        {
            Student user = _studentRepository.Get(id);
            user.GithubUsername = null;
            return _studentRepository.Update(user).To(s => new StudentFullProfileDto(s));
        }
    }
}