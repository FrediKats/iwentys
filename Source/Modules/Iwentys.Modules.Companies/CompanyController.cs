using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.Companies.Dtos;
using Iwentys.Modules.Companies.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Modules.Companies
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<CompanyInfoDto>>> Get(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            GetCompanies.Response response = await _mediator.Send(new GetCompanies.Query());

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<CompanyInfoDto>
                .ToIndexViewModel(response.Companies, paginationFilter));
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<CompanyInfoDto>> GetById(int id)
        {
            GetCompaniesById.Response response = await _mediator.Send(new GetCompaniesById.Query(id));
            return Ok(response.Company);
        }
    }
}