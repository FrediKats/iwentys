using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.PeerReview;

namespace Iwentys.PeerReview
{
    public class ProjectReviewRequestInfoDto
    {
        public ProjectReviewRequestInfoDto(ProjectReviewRequest reviewRequest) : this()
        {
            Id = reviewRequest.Id;
            Description = reviewRequest.Description;
            State = reviewRequest.State;
            CreationTimeUtc = reviewRequest.CreationTimeUtc;
            Project = new GithubRepositoryInfoDto(reviewRequest.Project);
            ReviewFeedbacks = reviewRequest.ReviewFeedbacks?.Select(rf => new ProjectReviewFeedbackInfoDto(rf)).ToList();
        }

        public ProjectReviewRequestInfoDto()
        {
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public ProjectReviewState State { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public GithubRepositoryInfoDto Project { get; set; }
        public ICollection<ProjectReviewFeedbackInfoDto> ReviewFeedbacks { get; set; }

        public static Expression<Func<ProjectReviewRequest, ProjectReviewRequestInfoDto>> FromEntity =>
            entity => new ProjectReviewRequestInfoDto
            {
                Id = entity.Id,
                Description = entity.Description,
                State = entity.State,
                CreationTimeUtc = entity.CreationTimeUtc,
                Project = new GithubRepositoryInfoDto
                {
                    Id = entity.Project.Id,
                    Owner = entity.Project.Owner,
                    Name = entity.Project.Name,
                    Description = entity.Project.Description,
                    Url = entity.Project.FullUrl,
                    StarCount = entity.Project.StarCount
                },
                ReviewFeedbacks = entity.ReviewFeedbacks.Select(rf => new ProjectReviewFeedbackInfoDto(rf)).ToList()
            };
    }
}