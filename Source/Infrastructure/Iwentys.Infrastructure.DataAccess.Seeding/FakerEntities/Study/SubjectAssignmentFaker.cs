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
            return new SubjectAssignment
            {
                Id = _faker.IndexVariable++ + 1,
                Title = _faker.Lorem.Word(),
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
            };
        }

        public SubjectAssignmentSubmitFeedbackArguments CreateFeedback(int submitId, FeedbackType feedbackType = FeedbackType.Approve)
        {
            return new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = _faker.Lorem.Word(),
                FeedbackType = feedbackType,
                Points = 0,
                SubjectAssignmentSubmitId = submitId
            };
        }

        public SubjectAssignmentSubmitCreateArguments CreateSubjectAssignmentSubmitCreateArguments(int assignmentId)
        {
            return new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _faker.Lorem.Word(),
                SubjectAssignmentId = assignmentId
            };
        }

        public SubjectAssignmentCreateArguments CreateSubjectAssignmentCreateArguments(int subjectId)
        {
            return new SubjectAssignmentCreateArguments
            {
                SubjectId = subjectId,
                Title = new Faker().Lorem.Word(),
                Description = new Faker().Lorem.Word(),
                Link = new Faker().Lorem.Word(),
                DeadlineUtc = DateTime.UtcNow.AddDays(1),
                Position = 1,
                AvailabilityState = AvailabilityState.Visible
            };
        }
    }
}