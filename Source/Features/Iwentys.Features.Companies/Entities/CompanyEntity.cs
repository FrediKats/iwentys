using System.Collections.Generic;

namespace Iwentys.Features.Companies.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<CompanyWorkerEntity> Workers { get; set; }
    }
}