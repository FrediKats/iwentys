using System.Collections.Generic;
using Iwentys.Core.Services;
using Iwentys.Models.Transferable.Companies;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.Api.Controllers
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
        public ActionResult<CompanyInfoResponse> Get(int id)
        {
            return Ok(_companyService.Get(id));
        }
    }
}