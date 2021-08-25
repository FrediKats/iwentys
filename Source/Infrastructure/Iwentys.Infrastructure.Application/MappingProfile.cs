using System.Linq;
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
using Iwentys.Infrastructure.Application.Modules.AccountManagment.Dtos;
using Iwentys.Infrastructure.Application.Modules.SubjectAssignments.Dtos;
using LanguageExt;
using Microsoft.AspNetCore.Mvc;

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
            CreateMapForMentors();
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

            CreateMap<Subject, SubjectAssignmentJournalItemDto>()
                .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));
        }

        public void CreateMapForMentors()
        {
            CreateMap<IGrouping<int, GroupSubject>, SubjectMentorsDto>()
                .ForMember(s => s.Name,
                           map =>
                               map.MapFrom(g => g.First().Subject.Title))
                .ForMember(s => s.Id,
                           map =>
                               map.MapFrom(g => g.Key))
                .ForMember(s => s.Groups,
                           map =>
                               map.MapFrom(g => g.Select(x => x)));

            CreateMap<GroupSubject, GroupMentorsDto>()
                .ForMember(gm=>gm.Name,
                    map=>
                        map.MapFrom(gs=>gs.StudyGroup.GroupName));
            CreateMap<UniversitySystemUser, MentorDto>();

            CreateMap<GroupSubjectMentor, MentorDto>()
                .ForMember(gs => gs.Id,
                               map=>
                                   map.MapFrom(gsm=>gsm.UserId))
                .ForMember(gs => gs.FirstName,
                           map =>
                               map.MapFrom(gsm => gsm.User.FirstName))
                .ForMember(gs => gs.MiddleName,
                           map =>
                               map.MapFrom(gsm => gsm.User.MiddleName))
                .ForMember(gs => gs.SecondName,
                           map =>
                               map.MapFrom(gsm => gsm.User.SecondName));
        }
    }
}