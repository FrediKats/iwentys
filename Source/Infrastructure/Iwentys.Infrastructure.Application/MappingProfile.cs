using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;

namespace Iwentys.Infrastructure.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentInfoDto>();
        }

        public void CreateMapForUsers()
        {
            CreateMap<IwentysUser, IwentysUserInfoDto>();
        }

        public void CreateMapForPeerReview()
        {
            CreateMap<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>();
        }

        public void CreateMapForProject()
        {
            CreateMap<GithubProject, GithubRepositoryInfoDto>();
        }
    }
}