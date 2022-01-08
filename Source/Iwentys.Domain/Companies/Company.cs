using System.Collections.Generic;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Companies.Dto;

namespace Iwentys.Domain.Companies
{
    public class Company
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public virtual List<CompanyWorker> Workers { get; init; }

        public Company()
        {
            Workers = new List<CompanyWorker>();
        }

        public static Company Create(IwentysUser creator, CompanyCreateArguments createArguments)
        {
            creator.EnsureIsAdmin();

            return new Company
            {
                Name = createArguments.Name,
                Latitude = createArguments.Latitude,
                Longitude = createArguments.Longitude
            };
        }

        public CompanyWorker NewRequest(IwentysUser worker, CompanyWorker currentWorkerState)
        {
            if (currentWorkerState is not null)
                throw new InnerLogicException("Student already request adding to company");

            var newWorker = new CompanyWorker
            {
                Company = this,
                Worker = worker,
                Type = CompanyWorkerType.Requested
            };
            Workers.Add(newWorker);
            return newWorker;
        }
    }
}