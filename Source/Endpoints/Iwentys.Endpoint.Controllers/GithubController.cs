using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
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

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<CodingActivityInfoResponse>>> GetByStudentId(int studentId)
        {
            GithubUser result = await _githubIntegrationService.User.FindGithubUser(studentId);

            if (result?.ContributionFullInfo is null)
                return Ok(new List<CodingActivityInfoResponse>());
            
            return result.ContributionFullInfo.PerMonthActivity().SelectToList(CodingActivityInfoResponse.Wrap);
        }

        [HttpGet(nameof(GetRepositoriesByStudentId))]
        public async Task<ActionResult<IReadOnlyList<GithubRepositoryInfoDto>>> GetRepositoriesByStudentId(int studentId)
        {
            IReadOnlyList<GithubRepositoryInfoDto> result = await _githubIntegrationService.Repository.GetStudentRepositories(studentId);
            return Ok(result);
        }
    }
}