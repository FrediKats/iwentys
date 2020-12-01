using System.Collections.Generic;
using Iwentys.Features.GithubIntegration.Services;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.ViewModels;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private readonly GithubUserDataService _githubUserDataService;

        public GithubController(GithubUserDataService githubUserDataService)
        {
            _githubUserDataService = githubUserDataService;
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<CodingActivityInfoResponse>>> GetForStudent(int studentId)
        {
            GithubUserEntity result = await _githubUserDataService.Read(studentId);

            if (result?.ContributionFullInfo is null)
                return Ok(new List<CodingActivityInfoResponse>());
            
            return result.ContributionFullInfo.PerMonthActivity().SelectToList(CodingActivityInfoResponse.Wrap);
        }
    }
}