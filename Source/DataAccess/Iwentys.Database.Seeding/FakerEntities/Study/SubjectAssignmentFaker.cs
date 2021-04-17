﻿using System;
using Bogus;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;

namespace Iwentys.Database.Seeding.FakerEntities.Study
{
    public class SubjectAssignmentFaker
    {
        public static readonly SubjectAssignmentFaker Instance = new SubjectAssignmentFaker();

        private readonly Faker _faker = new Faker();

        public SubjectAssignment Create(int subjectId, Assignment assignment)
        {
            return new SubjectAssignment
            {
                Id = _faker.IndexVariable++ + 1,
                AssignmentId = assignment.Id,
                SubjectId = subjectId
            };
        }

        public SubjectAssignmentSubmit CreateSubjectAssignmentSubmit(int subjectAssignmentId, int studentId)
        {
            return new SubjectAssignmentSubmit
            {
                Id = _faker.IndexVariable++ + 1,
                SubjectAssignmentId = subjectAssignmentId,
                StudentId = studentId,
                SubmitTimeUtc = DateTime.UtcNow
            };
        }

        public SubjectAssignmentSubmitFeedbackArguments CreateFeedback(FeedbackType feedbackType = FeedbackType.Approve)
        {
            return new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = _faker.Lorem.Word(),
                FeedbackType = feedbackType
            };
        }

        public SubjectAssignmentSubmitCreateArguments CreateSubjectAssignmentSubmitCreateArguments()
        {
            return new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _faker.Lorem.Word()
            };
        }

        public SubjectAssignmentCreateArguments CreateSubjectAssignmentCreateArguments()
        {
            return new SubjectAssignmentCreateArguments
            {
                Title = new Faker().Lorem.Word(),
                Description = new Faker().Lorem.Word(),
                Link = new Faker().Lorem.Word(),
                DeadlineUtc = DateTime.UtcNow.AddDays(1)
            };
        }
    }
}