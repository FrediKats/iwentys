using System;
using Bogus;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Enums;
using Iwentys.Domain.SubjectAssignments.Models;

namespace Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Study
{
    public class SubjectAssignmentFaker
    {
        public static readonly SubjectAssignmentFaker Instance = new SubjectAssignmentFaker();

        private readonly Faker _faker = new Faker();

        public SubjectAssignment Create(int subjectId, int authorId)
        {
            var id = _faker.IndexVariable++ + 1;
            return new SubjectAssignment
            {
                Id = id,
                Title = $"Homework #{id}",
                Description = _faker.Lorem.Paragraph(),
                Link = _faker.Internet.Url(),
                DeadlineTimeUtc = DateTime.UtcNow.AddMonths(1),
                Position = 1,
                SubjectId = subjectId,
                AuthorId = authorId,
                AvailabilityState = AvailabilityState.Visible
            };
        }

        public SubjectAssignmentSubmit CreateSubjectAssignmentSubmit(int subjectAssignmentId, int studentId)
        {
            return new SubjectAssignmentSubmit
            {
                Id = _faker.IndexVariable++ + 1,
                SubjectAssignmentId = subjectAssignmentId,
                StudentId = studentId,
                SubmitTimeUtc = DateTime.UtcNow,
                StudentDescription = _faker.Lorem.Paragraph(1),
            };
        }

        public SubjectAssignmentSubmit CreateSubjectAssignmentSubmitWithFeedback(int subjectAssignmentId, int studentId)
        {
            return new SubjectAssignmentSubmit
            {
                Id = _faker.IndexVariable++ + 1,
                SubjectAssignmentId = subjectAssignmentId,
                StudentId = studentId,
                SubmitTimeUtc = DateTime.UtcNow,
                StudentDescription = _faker.Lorem.Paragraph(1),

                Comment = _faker.Lorem.Paragraph(1),
                Points = 5,
                ApproveTimeUtc = DateTime.UtcNow,
            };
        }

        public SubjectAssignmentSubmitFeedbackArguments CreateFeedback(int submitId, FeedbackType feedbackType = FeedbackType.Approve)
        {
            return new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = _faker.Lorem.Paragraph(1),
                FeedbackType = feedbackType,
                Points = 0,
                SubjectAssignmentSubmitId = submitId
            };
        }

        public SubjectAssignmentSubmitCreateArguments CreateSubjectAssignmentSubmitCreateArguments(int assignmentId)
        {
            return new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _faker.Lorem.Paragraph(1),
                SubjectAssignmentId = assignmentId
            };
        }

        public SubjectAssignmentCreateArguments CreateSubjectAssignmentCreateArguments(int subjectId)
        {
            return new SubjectAssignmentCreateArguments
            {
                SubjectId = subjectId,
                Title = _faker.Lorem.Word(),
                Description = _faker.Lorem.Paragraph(1),
                Link = _faker.Internet.Url(),
                DeadlineUtc = DateTime.UtcNow.AddDays(1),
                Position = 1,
                AvailabilityState = AvailabilityState.Visible
            };
        }
    }
}