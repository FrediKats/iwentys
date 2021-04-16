using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Domain.Enums;

namespace Iwentys.Domain.Models
{
    public record CompanyInfoDto
    {
        public CompanyInfoDto(Company company)
            : this(
                company.Id,
                company.Name,
                company.Latitude,
                company.Longitude,
                company.Workers?.Where(w => w.Type == CompanyWorkerType.Accepted).SelectToList(w => new IwentysUserInfoDto(w.Worker)))
        {
        }

        public CompanyInfoDto(int id, string name, double latitude, double longitude, List<IwentysUserInfoDto> workers)
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
        public List<IwentysUserInfoDto> Workers { get; init; }
    }
}