using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.InterestTags.Models;
using Iwentys.Features.InterestTags.Services;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Gamification
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

        [HttpGet("get-all")]
        public async Task<ActionResult<List<InterestTagDto>>> GetAllTags()
        {
            List<InterestTagDto> result = await _interestTagService.GetAllTags();
            return Ok(result);
        }

        [HttpGet("get-students")]
        public async Task<ActionResult<List<InterestTagDto>>> GetStudentTags(int studentId)
        {
            List<InterestTagDto> result = await _interestTagService.GetUserTags(studentId);
            return result;
        }

        [HttpGet("add-students")]
        public async Task<ActionResult> AddStudentTag(int studentId, int tagId)
        {
            await _interestTagService.AddUserTag(studentId, tagId);
            return Ok();
        }
        
        [HttpGet("remove-students")]
        public async Task<ActionResult> RemoveStudentTag(int studentId, int tagId)
        {
            await _interestTagService.RemoveUserTag(studentId, tagId);
            return Ok();
        }
    }
}