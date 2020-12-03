using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Companies.Services;
using Iwentys.Features.Companies.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CompanyViewModel>> Get()
        {
            return Ok(_companyService.Get());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyViewModel>> Get(int id)
        {
            CompanyViewModel company = await _companyService.Get(id);
            return Ok(company);
        }
    }
}