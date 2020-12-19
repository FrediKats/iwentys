using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Companies.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
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

        [HttpGet]
        public async Task<ActionResult<List<CompanyInfoDto>>> Get()
        {
            List<CompanyInfoDto> companies = await _companyService.GetAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyInfoDto>> Get(int id)
        {
            CompanyInfoDto company = await _companyService.GetAsync(id);
            return Ok(company);
        }
    }
}