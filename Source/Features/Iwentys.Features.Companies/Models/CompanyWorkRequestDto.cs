using Iwentys.Common.Exceptions;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Companies.Models
{
    public record CompanyWorkRequestDto(CompanyInfoDto Company, StudentEntity Worker)
    {
        public static CompanyWorkRequestDto Create(CompanyWorkerEntity workerEntity)
        {
            if (workerEntity.Type != CompanyWorkerType.Requested)
                throw new InnerLogicException($"Invalid operation, cannot convert non-request type of {nameof(CompanyWorkerEntity)} to {nameof(CompanyWorkRequestDto)}");

            return new CompanyWorkRequestDto(new CompanyInfoDto(workerEntity.CompanyEntity), workerEntity.Worker);
        }
    }
}