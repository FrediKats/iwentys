using System.Linq;
using Bogus;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public class StudyTestCaseContext
    {
        private readonly TestCaseContext _context;

        public StudyTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public GroupProfileResponseDto WithStudyGroup()
        {
            var studyGroup = new StudyGroup()
            {
                GroupName = new Faker().Lorem.Word()
            };


            _context.UnitOfWork.GetRepository<StudyGroup>().InsertAsync(studyGroup).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            return _context.UnitOfWork
                .GetRepository<StudyGroup>()
                .Get()
                .Where(sg => sg.Id == studyGroup.Id)
                .Select(GroupProfileResponseDto.FromEntity)
                .Single();
        }

        public AuthorizedUser WithNewStudent(GroupProfileResponseDto studyGroup)
        {
            int id = RandomProvider.Random.Next(999999);

            var userInfo = new Student
            {
                Id = id,
                GithubUsername = $"{TestCaseContext.Constants.GithubUsername}{id}",
                BarsPoints = 1000,
            };

            _context.UnitOfWork.GetRepository<Student>().InsertAsync(userInfo).Wait();
            _context.UnitOfWork.GetRepository<StudyGroupMember>().InsertAsync(new StudyGroupMember {StudentId = userInfo.Id, GroupId = studyGroup.Id}).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            var user = AuthorizedUser.DebugAuth(userInfo.Id);
            return user;
        }
    }
}