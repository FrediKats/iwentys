using AutoMapper;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;

namespace Iwentys.Modules.AccountManagement
{
    public class AccountManagementMappingProfile : Profile
    {
        public AccountManagementMappingProfile()
        {
            CreateMap<Student, StudentInfoDto>();
        }
    }
}