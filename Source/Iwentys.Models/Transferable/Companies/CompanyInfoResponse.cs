using System.Collections.Generic;
using Iwentys.Models.Entities;

namespace Iwentys.Models.Transferable.Companies
{
    public class CompanyInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<StudentEntity> Workers { get; set; }

        public static CompanyInfoResponse Create(CompanyEntity companyEntity)
        {
            return Create(companyEntity, new List<StudentEntity>());
        }

        public static CompanyInfoResponse Create(CompanyEntity companyEntity, List<StudentEntity> workers)
        {
            return new CompanyInfoResponse
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