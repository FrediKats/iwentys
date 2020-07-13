using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Transferable.Companies;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public IEnumerable<CompanyInfoDto> Get()
        {
            return _companyService.Get();
        }

        [HttpGet("{id}")]
        public CompanyInfoDto Get(int id)
        {
            return _companyService.Get(id);
        }
    }
}