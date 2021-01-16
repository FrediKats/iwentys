using System;
using System.Linq;
using Bogus;
using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Enums;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Models.Students;
using Iwentys.Features.Study.SubjectAssignments.Enums;
using Iwentys.Features.Study.SubjectAssignments.Models;

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
            var studyGroup = new StudyGroup
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
            _context.UnitOfWork.CommitAsync().Wait();
            return subject;
        }

        public GroupSubject WithGroupSubject(GroupProfileResponseDto studyGroup, Subject subject, AuthorizedUser teacher = null)
        {
            var groupSubject = new GroupSubject
            {
                StudyGroupId = studyGroup.Id,
                SubjectId = subject.Id,
                StudySemester = StudySemester.Y21H1,
                LectorTeacherId = teacher?.Id
            };
            _context.UnitOfWork.GetRepository<GroupSubject>().InsertAsync(groupSubject).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            return groupSubject;
        }

        public SubjectAssignmentDto WithSubjectAssignment(AuthorizedUser user, GroupSubject groupSubject)
        {
            var subjectAssignmentCreateArguments = new SubjectAssignmentCreateArguments
            {
                Title = new Faker().Lorem.Word(),
                Description = new Faker().Lorem.Word(),
                Link = new Faker().Lorem.Word(),
                DeadlineUtc = DateTime.UtcNow.AddDays(1)
            };

            return _context.SubjectAssignmentService.CreateSubjectAssignment(user, groupSubject.SubjectId, subjectAssignmentCreateArguments).Result;
        }

        public SubjectAssignmentSubmitDto WithSubjectAssignmentSubmit(AuthorizedUser user, SubjectAssignmentDto assignment)
        {
            var subjectAssignmentCreateArguments = new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = new Faker().Lorem.Word()
            };

            return _context.SubjectAssignmentService.SendSubmit(user, assignment.Id, subjectAssignmentCreateArguments).Result;
        }

        public void WithSubjectAssignmentSubmitFeedback(AuthorizedUser user, SubjectAssignmentSubmitDto submit, FeedbackType feedbackType = FeedbackType.Approve)
        {
            var subjectAssignmentCreateArguments = new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = new Faker().Lorem.Word(),
                FeedbackType = feedbackType
            };

            _context.SubjectAssignmentService.SendFeedback(user, submit.Id, subjectAssignmentCreateArguments).Wait();
        }

        public AuthorizedUser WithNewStudent(GroupProfileResponseDto studyGroup)
        {
            StudentCreateArguments createArguments = UsersFaker.Instance.Students.Generate();
            createArguments.Id = UsersFaker.Instance.GetIdentifier();
            createArguments.GroupId = studyGroup.Id;

            StudentInfoDto studentInfoDto = _context.StudentService.Create(createArguments).Result;

            AuthorizedUser user = AuthorizedUser.DebugAuth(studentInfoDto.Id);
            return user;
        }
    }
}