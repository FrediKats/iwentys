using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.PeerReview.Enums;
using Iwentys.Features.PeerReview.Models;

namespace Iwentys.Features.PeerReview.Entities
{
    public class ProjectReviewRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public ProjectReviewState State { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public ProjectReviewVisibility Visibility { get; set; }
        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public long ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public virtual ICollection<ProjectReviewFeedback> ReviewFeedbacks { get; set; }

        public static Expression<Func<ProjectReviewRequest, bool>> IsVisibleTo(AuthorizedUser user) =>
            request => request.Visibility == ProjectReviewVisibility.Open
            || request.AuthorId == user.Id;

        public static ProjectReviewRequest Create(AuthorizedUser author, GithubProject githubProject, ReviewRequestCreateArguments createArguments)
        {
            if (githubProject.OwnerUserId != author.Id) throw new InnerLogicException("Project do not belong to user");

            return new ProjectReviewRequest
            {
                Description = createArguments.Description,
                State = ProjectReviewState.Requested,
                CreationTimeUtc = DateTime.UtcNow,
                LastUpdateTimeUtc = DateTime.UtcNow,
                Visibility = createArguments.Visibility,
                ProjectId = githubProject.Id,
                AuthorId = author.Id
            };
        }

        public ProjectReviewFeedback CreateFeedback(AuthorizedUser author, ReviewFeedbackCreateArguments createArguments)
        {
            if (State == ProjectReviewState.Finished)
                throw InnerLogicException.PeerReviewExceptions.ReviewAlreadyClosed(Id);

            LastUpdateTimeUtc = DateTime.UtcNow;

            return new ProjectReviewFeedback
            {
                AuthorId = author.Id,
                Description = createArguments.Description,
                Summary = createArguments.Summary,
                CreationTimeUtc = DateTime.UtcNow,
                ReviewRequestId = Id
            };
        }

        public void FinishReview(IwentysUser user)
        {
            if (user.Id != AuthorId && !user.IsAdmin) throw InnerLogicException.NotEnoughPermissionFor(user.Id);

            if (State == ProjectReviewState.Finished)
                throw InnerLogicException.PeerReviewExceptions.ReviewAlreadyClosed(Id);

            State = ProjectReviewState.Finished;
        }
    }
}