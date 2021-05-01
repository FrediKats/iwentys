using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Companies
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
            private readonly IGenericRepository<CompanyWorker> _companyWorkerRepository;
            private readonly IGenericRepository<IwentysUser> _userRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _companyRepository = _unitOfWork.GetRepository<Company>();
                _companyWorkerRepository = _unitOfWork.GetRepository<CompanyWorker>();
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
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