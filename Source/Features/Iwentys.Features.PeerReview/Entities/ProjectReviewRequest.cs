using System;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.PeerReview.Enums;

namespace Iwentys.Features.PeerReview.Entities
{
    public class ProjectReviewRequest
    {
        public int Id { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string Description { get; set; }
        public ProjectReviewState State { get; set; }

        public int ProjectId { get; set; }
        public virtual GithubProject Project { get; set; }
        
    }
}