using AutoMapper;
using Iwentys.Domain.PeerReview;
using Iwentys.Modules.PeerReview.Dtos;

namespace Iwentys.Modules.PeerReview
{
    public class PeerReviewMappingProfile : Profile
    {
        public PeerReviewMappingProfile()
        {
            CreateMap<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>();
        }
    }
}