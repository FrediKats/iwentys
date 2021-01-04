using System;
using Iwentys.Features.PeerReview.Entities;
using Iwentys.Features.PeerReview.Enums;

namespace Iwentys.Features.PeerReview.Models
{
    public class ProjectReviewRequestInfoDto
    {
        //TODO: need to add project info, title, tags
        public int Id { get; set; }
        public string Description { get; set; }
        public ProjectReviewState State { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public long ProjectId { get; set; }

        public ProjectReviewRequestInfoDto(ProjectReviewRequest reviewRequest) : this()
        {
            Id = reviewRequest.Id;
            Description = reviewRequest.Description;
            State = reviewRequest.State;
            CreationTimeUtc = reviewRequest.CreationTimeUtc;
            ProjectId = reviewRequest.ProjectId;
        }

        public ProjectReviewRequestInfoDto()
        {
        }
    }
}