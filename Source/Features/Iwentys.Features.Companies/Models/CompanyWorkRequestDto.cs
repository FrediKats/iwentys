using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Companies.Models
{
    public class CompanyWorkRequestDto
    {
        public CompanyViewModel Company { get; set; }
        public StudentEntity Worker { get; set; }

        public static CompanyWorkRequestDto Create(CompanyWorkerEntity workerEntity)
        {
            if (workerEntity.Type != CompanyWorkerType.Requested)
                throw new InnerLogicException($"Invalid operation, cannot convert non-request type of {nameof(CompanyWorkerEntity)} to {nameof(CompanyWorkRequestDto)}");

            return new CompanyWorkRequestDto
            {
                Company = workerEntity.CompanyEntity.To(CompanyViewModel.Create),
                Worker = workerEntity.Worker
            };
        }
    }
}