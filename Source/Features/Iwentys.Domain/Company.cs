using System.Collections.Generic;
using Iwentys.Domain.Models;

namespace Iwentys.Domain
{
    public class Company
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public virtual List<CompanyWorker> Workers { get; init; }

        public static Company Create(SystemAdminUser creator, CompanyCreateArguments createArguments)
        {
            return new Company
            {
                Name = createArguments.Name,
                Latitude = createArguments.Latitude,
                Longitude = createArguments.Longitude
            };
        }
    }
}