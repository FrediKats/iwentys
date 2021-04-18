using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Models;
using Iwentys.Features.Extended.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Features.Extended.Companies
{
    [Route("api/companies")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<CompanyInfoDto>>> Get()
        {
            List<CompanyInfoDto> companies = await _companyService.Get();
            return Ok(companies);
        }

        [HttpGet(nameof(GetById))]
        public async Task<ActionResult<CompanyInfoDto>> GetById(int id)
        {
            CompanyInfoDto company = await _companyService.Get(id);
            return Ok(company);
        }
    }
}