using AutoMapper;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;

namespace Iwentys.FeatureBase
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentInfoDto>();
        }
    }
}