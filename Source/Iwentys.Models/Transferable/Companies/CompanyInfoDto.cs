using System;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Companies
{
    public class CompanyInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public StudentEntity[] Workers { get; set; }

        public static CompanyInfoDto Create(CompanyEntity companyEntity)
        {
            return Create(companyEntity, Array.Empty<StudentEntity>());
        }

        public static CompanyInfoDto Create(CompanyEntity companyEntity, StudentEntity[] workers)
        {
            return new CompanyInfoDto
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