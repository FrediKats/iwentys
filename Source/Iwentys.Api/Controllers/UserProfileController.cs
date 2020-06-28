using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public IEnumerable<UserProfile> Get()
        {
            return _userProfileService.Get();
        }

        [HttpGet("{id}")]
        public UserProfile Get(int id)
        {
            return _userProfileService.Get(id);
        }
    }
}