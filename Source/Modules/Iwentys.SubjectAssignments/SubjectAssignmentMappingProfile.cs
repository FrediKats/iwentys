using AutoMapper;
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
    }
}