using System;
using System.Collections.Generic;
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

        public long ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public virtual ICollection<ProjectReviewFeedback> ReviewFeedbacks { get; set; }

        public static ProjectReviewRequest Create(AuthorizedUser author, ReviewRequestCreateArguments createArguments)
        {
            //TODO: ensure project belong to author
            return new ProjectReviewRequest
            {
                Description = createArguments.Description,
                State = ProjectReviewState.Requested,
                CreationTimeUtc = DateTime.UtcNow,
                LastUpdateTimeUtc = DateTime.UtcNow,
                ProjectId = createArguments.ProjectId
            };
        }

        public ProjectReviewFeedback CreateFeedback(AuthorizedUser author, ReviewFeedbackCreateArguments createArguments)
        {
            //TODO: validate state
            if (State == ProjectReviewState.Finished)
                throw new InnerLogicException("Request already finished");

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
            if (user.Id != Project.OwnerUserId && !user.IsAdmin)
            {
                throw InnerLogicException.NotEnoughPermissionFor(user.Id);
            }

            if (State == ProjectReviewState.Finished)
                throw new InnerLogicException("Request already finished");

            State = ProjectReviewState.Finished;
        }
    }
}