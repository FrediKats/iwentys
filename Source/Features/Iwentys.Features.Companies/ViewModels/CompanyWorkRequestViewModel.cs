using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Models.Entities;

namespace Iwentys.Features.Companies.ViewModels
{
    public class CompanyWorkRequestViewModel
    {
        public CompanyViewModel Company { get; set; }
        public StudentEntity Worker { get; set; }

        public static CompanyWorkRequestViewModel Create(CompanyWorkerEntity workerEntity)
        {
            if (workerEntity.Type != CompanyWorkerType.Requested)
                throw new InnerLogicException($"Invalid operation, cannot convert non-request type of {nameof(CompanyWorkerEntity)} to {nameof(CompanyWorkRequestViewModel)}");

            return new CompanyWorkRequestViewModel
            {
                Company = workerEntity.CompanyEntity.To(CompanyViewModel.Create),
                Worker = workerEntity.Worker
            };
        }
    }
}