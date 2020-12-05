using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Companies.Models
{
    public record CompanyInfoDto(int Id, string Name, double Latitude, double Longitude, List<StudentInfoDto> Workers)
    {
        public CompanyInfoDto(CompanyEntity companyEntity)
            : this(
                companyEntity.Id,
                companyEntity.Name,
                companyEntity.Latitude,
                companyEntity.Longitude,
                companyEntity.Workers?.SelectToList(w => new StudentInfoDto(w.Worker)))
        {
        }
    }
}