using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Core.Services;
using Iwentys.Models.Transferable.Companies;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Server.Controllers
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
        public ActionResult<IEnumerable<CompanyInfoResponse>> Get()
        {
            return Ok(_companyService.Get());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyInfoResponse>> Get(int id)
        {
            CompanyInfoResponse company = await _companyService.Get(id);
            return Ok(company);
        }
    }
}