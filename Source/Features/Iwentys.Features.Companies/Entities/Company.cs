using System.Collections.Generic;

namespace Iwentys.Features.Companies.Entities
{
    public class Company
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public virtual List<CompanyWorker> Workers { get; init; }
    }
}