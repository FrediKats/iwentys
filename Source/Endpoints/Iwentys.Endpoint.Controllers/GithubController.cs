using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private readonly GithubIntegrationService _githubIntegrationService;

        public GithubController(GithubIntegrationService githubIntegrationService)
        {
            _githubIntegrationService = githubIntegrationService;
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<List<CodingActivityInfoResponse>>> GetForStudent(int studentId)
        {
            GithubUser result = await _githubIntegrationService.UserApiApiAccessor.FindGithubUser(studentId);

            if (result?.ContributionFullInfo is null)
                return Ok(new List<CodingActivityInfoResponse>());
            
            return result.ContributionFullInfo.PerMonthActivity().SelectToList(CodingActivityInfoResponse.Wrap);
        }

        [HttpGet("student/{studentId}/repository")]
        public async Task<ActionResult<IReadOnlyList<GithubRepositoryInfoDto>>> GetStudentRepositories(int studentId)
        {
            IReadOnlyList<GithubRepositoryInfoDto> result = await _githubIntegrationService.GetStudentRepositories(studentId);
            return Ok(result);
        }
    }
}