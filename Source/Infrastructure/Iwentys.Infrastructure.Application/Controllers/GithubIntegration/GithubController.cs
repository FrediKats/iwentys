using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application.Controllers.GithubIntegration
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

        // TODO: Ask if need to add pagination and how to implement it
        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<CodingActivityInfoResponse>>> GetByStudentId(int studentId)
        {
            GithubUser result = await _githubIntegrationService.User.FindGithubUser(studentId);

            if (result?.ContributionFullInfo is null)
                return Ok(new List<CodingActivityInfoResponse>());
            
            return result.ContributionFullInfo.PerMonthActivity().SelectToList(CodingActivityInfoResponse.Wrap);
        }

        [HttpGet(nameof(GetRepositoriesByStudentId))]
        public async Task<ActionResult<IReadOnlyList<GithubRepositoryInfoDto>>> GetRepositoriesByStudentId(
            [FromQuery] int studentId,
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            IReadOnlyList<GithubRepositoryInfoDto> result = await _githubIntegrationService.Repository.GetStudentRepositories(studentId);
            
            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<GithubRepositoryInfoDto>
                .ToIndexViewModel(result.ToList(), paginationFilter));
        }
    }
}