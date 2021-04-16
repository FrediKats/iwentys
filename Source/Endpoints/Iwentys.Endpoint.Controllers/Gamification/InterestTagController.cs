using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Models;
using Iwentys.Features.InterestTags.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Gamification
{
    [Route("api/InterestTag")]
    [ApiController]
    public class InterestTagController : ControllerBase
    {
        private readonly InterestTagService _interestTagService;

        public InterestTagController(InterestTagService interestTagService)
        {
            _interestTagService = interestTagService;
        }

        [HttpGet(nameof(Get))]
        public async Task<ActionResult<List<InterestTagDto>>> Get()
        {
            List<InterestTagDto> result = await _interestTagService.GetAllTags();
            return Ok(result);
        }

        [HttpGet(nameof(GetByStudentId))]
        public async Task<ActionResult<List<InterestTagDto>>> GetByStudentId(int studentId)
        {
            List<InterestTagDto> result = await _interestTagService.GetUserTags(studentId);
            return result;
        }

        [HttpGet(nameof(Add))]
        public async Task<ActionResult> Add(int studentId, int tagId)
        {
            await _interestTagService.AddUserTag(studentId, tagId);
            return Ok();
        }
        
        [HttpGet(nameof(Remove))]
        public async Task<ActionResult> Remove(int studentId, int tagId)
        {
            await _interestTagService.RemoveUserTag(studentId, tagId);
            return Ok();
        }
    }
}