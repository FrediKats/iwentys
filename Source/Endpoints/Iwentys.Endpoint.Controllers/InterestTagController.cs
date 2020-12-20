using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Gamification.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class InterestTagController : ControllerBase
    {
        private readonly InterestTagService _interestTagService;

        public InterestTagController(InterestTagService interestTagService)
        {
            _interestTagService = interestTagService;
        }

        [HttpGet]
        public async Task<ActionResult<List<InterestTagDto>>> GetForStudent()
        {
            List<InterestTagDto> result = await _interestTagService.GetAllTags();
            return Ok(result);
        }
    }
}