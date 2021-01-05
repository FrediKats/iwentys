using System;
using System.Linq.Expressions;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Enums;

namespace Iwentys.Features.PeerReview.Models
{
    public class ProjectReviewFeedbackInfoDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public ReviewFeedbackSummary Summary { get; set; }

        public IwentysUserInfoDto Author { get; set; }

        public ProjectReviewFeedbackInfoDto(ProjectReviewFeedback reviewFeedback) : this()
        {
            Id = reviewFeedback.Id;
            Description = reviewFeedback.Description;
            CreationTimeUtc = reviewFeedback.CreationTimeUtc;
            Summary = reviewFeedback.Summary;
            Author = new IwentysUserInfoDto(reviewFeedback.Author);
        }

        public ProjectReviewFeedbackInfoDto()
        {
        }

        public static Expression<Func<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>> FromEntity =>
            entity => new ProjectReviewFeedbackInfoDto
            {
                Id = entity.Id,
                Description = entity.Description,
                CreationTimeUtc = entity.CreationTimeUtc,
                Summary = entity.Summary,
                Author = new IwentysUserInfoDto(entity.Author),
            };
    }
}