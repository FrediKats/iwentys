using System;
using Iwentys.Features.AccountManagement.Domain;
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

        public long ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }

        public static ProjectReviewRequest Create(AuthorizedUser author, ReviewRequestCreateArguments createArguments)
        {
            //TODO: ensure project belong to author
            return new ProjectReviewRequest
            {
                Description = createArguments.Description,
                State = ProjectReviewState.Requested,
                CreationTimeUtc = DateTime.UtcNow,
                ProjectId = createArguments.ProjectId
            };
        }
    }
}