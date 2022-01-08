using AutoMapper;
using Iwentys.Domain.PeerReview;

namespace Iwentys.PeerReview
{
    public class PeerReviewMappingProfile : Profile
    {
        public PeerReviewMappingProfile()
        {
            CreateMap<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>();
        }
    }
}