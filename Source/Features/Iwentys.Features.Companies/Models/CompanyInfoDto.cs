using System.Collections.Generic;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Companies.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<StudentEntity> Workers { get; set; }

        public static CompanyViewModel Create(CompanyEntity companyEntity)
        {
            return Create(companyEntity, new List<StudentEntity>());
        }

        public static CompanyViewModel Create(CompanyEntity companyEntity, List<StudentEntity> workers)
        {
            return new CompanyViewModel
            {
                Id = companyEntity.Id,
                Name = companyEntity.Name,
                Latitude = companyEntity.Latitude,
                Longitude = companyEntity.Longitude,
                Workers = workers
            };
        }
    }
}