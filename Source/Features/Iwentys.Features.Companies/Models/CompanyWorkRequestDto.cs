using System;
using System.Linq.Expressions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Companies.Entities;

namespace Iwentys.Features.Companies.Models
{
    public record CompanyWorkRequestDto
    {
        public CompanyWorkRequestDto(CompanyInfoDto company, IwentysUser worker)
        {
            Company = company;
            Worker = worker;
        }

        public CompanyWorkRequestDto()
        {
        }

        //TODO: it is not work
        public static Expression<Func<CompanyWorker, CompanyWorkRequestDto>> FromEntity =>
            entity => new CompanyWorkRequestDto(new CompanyInfoDto(entity.Company), entity.Worker);
        
        public CompanyInfoDto Company { get; init; }
        public IwentysUser Worker { get; init; }
    }
}