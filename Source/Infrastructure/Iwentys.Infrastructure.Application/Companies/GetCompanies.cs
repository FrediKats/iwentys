using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Companies;
using Iwentys.Domain.Companies.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Companies
{
    public class GetCompanies
    {
        public class Query : IRequest<Response>
        {

        }

        public class Response
        {
            public Response(List<CompanyInfoDto> companies)
            {
                Companies = companies;
            }

            public List<CompanyInfoDto> Companies { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<Company> _companyRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _companyRepository = _unitOfWork.GetRepository<Company>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _companyRepository
                    .Get()
                    .Select(entity => new CompanyInfoDto(entity))
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}