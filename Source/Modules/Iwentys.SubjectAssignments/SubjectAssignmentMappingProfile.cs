using AutoMapper;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;

namespace Iwentys.SubjectAssignments;

public class SubjectAssignmentMappingProfile : Profile
{
    public SubjectAssignmentMappingProfile()
    {
        CreateMapForSubjectAssignment();
    }

    public void CreateMapForSubjectAssignment()
    {
        CreateMap<SubjectAssignment, SubjectAssignmentDto>();
        CreateMap<SubjectAssignmentSubmit, SubjectAssignmentSubmitDto>();

        CreateMap<Subject, SubjectAssignmentJournalItemDto>()
            .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));
    }
}