using System.Linq;
using Bogus;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Enums;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.SubjectAssignments.Models;
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

        public Subject WithSubject()
        {
            var subject = new Subject
            {
                Name = new Faker().Lorem.Word()
            };

            _context.UnitOfWork.GetRepository<Subject>().InsertAsync(subject).Wait();
            return subject;
        }

        public GroupSubject WithGroupSubject(GroupProfileResponseDto studyGroup, Subject subject)
        {
            var groupSubject = new GroupSubject
            {
                StudyGroupId = studyGroup.Id,
                SubjectId = subject.Id,
                StudySemester = StudySemester.Y21H1
            };
            _context.UnitOfWork.GetRepository<GroupSubject>().InsertAsync(groupSubject).Wait();
            return groupSubject;
        }

        public SubjectAssignmentDto WithSubjectAssignment(AuthorizedUser user, GroupSubject groupSubject)
        {
            var subjectAssignmentCreateArguments = new SubjectAssignmentCreateArguments
            {
                Title = new Faker().Lorem.Word(),
                Description = new Faker().Lorem.Word(),
                Link = new Faker().Lorem.Word(),
            };

            return _context.SubjectAssignmentService.CreateSubjectAssignment(user, groupSubject.SubjectId, subjectAssignmentCreateArguments).Result;
        }

        public void WithSubjectAssignmentSubmit(AuthorizedUser user, SubjectAssignmentDto assignment)
        {
            var subjectAssignmentCreateArguments = new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = new Faker().Lorem.Word(),
            };

            _context.SubjectAssignmentService.SendSubmit(user, assignment.Id, subjectAssignmentCreateArguments).Wait();
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