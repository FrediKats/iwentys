using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.Students.Models;

namespace Iwentys.Features.Companies.Models
{
    public record CompanyInfoDto
    {
        public CompanyInfoDto(CompanyEntity companyEntity)
            : this(
                companyEntity.Id,
                companyEntity.Name,
                companyEntity.Latitude,
                companyEntity.Longitude,
                companyEntity.Workers?.Where(w => w.Type == CompanyWorkerType.Accepted).SelectToList(w => new StudentInfoDto(w.Worker)))
        {
        }

        public CompanyInfoDto(int id, string name, double latitude, double longitude, List<StudentInfoDto> workers)
        {
            Id = id;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Workers = workers;
        }

        public CompanyInfoDto()
        {
        }
        
        public int Id { get; init; }
        public string Name { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public List<StudentInfoDto> Workers { get; init; }
    }
}