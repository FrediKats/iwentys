using AutoMapper;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Domain.PeerReview;
using Iwentys.Domain.PeerReview.Dto;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Models;

namespace Iwentys.Infrastructure.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMapForUsers();
            CreateMapForPeerReview();
            CreateMapForProject();
            CreateMapForSubjectAssignment();

        }

        public void CreateMapForUsers()
        {
            CreateMap<IwentysUser, IwentysUserInfoDto>();
            CreateMap<Student, StudentInfoDto>();

        }

        public void CreateMapForPeerReview()
        {
            CreateMap<ProjectReviewFeedback, ProjectReviewFeedbackInfoDto>();
        }

        public void CreateMapForProject()
        {
            CreateMap<GithubProject, GithubRepositoryInfoDto>();
        }

        public void CreateMapForSubjectAssignment()
        {
            CreateMap<SubjectAssignment, SubjectAssignmentDto>();
            CreateMap<SubjectAssignmentSubmit, SubjectAssignmentSubmitDto>();
        }
    }
}