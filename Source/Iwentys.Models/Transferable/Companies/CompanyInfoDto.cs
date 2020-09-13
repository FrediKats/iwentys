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

        public static CompanyInfoDto Create(Company company)
        {
            return Create(company, Array.Empty<StudentEntity>());
        }

        public static CompanyInfoDto Create(Company company, StudentEntity[] workers)
        {
            return new CompanyInfoDto
            {
                Id = company.Id,
                Name = company.Name,
                Latitude = company.Latitude,
                Longitude = company.Longitude,
                Workers = workers
            };
        }
    }
}