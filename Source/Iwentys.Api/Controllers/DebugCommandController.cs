using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Iwentys.Core;
using Iwentys.Core.Auth;
using Iwentys.Core.GoogleTableIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly GoogleTableUpdateService _googleTableUpdateService;
        private readonly ILogger<DebugCommandController> _logger;
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IStudentService _studentService;

        public DebugCommandController(ILogger<DebugCommandController> logger, DatabaseAccessor databaseAccessor, IStudentService studentService)
        {
            _logger = logger;
            _databaseAccessor = databaseAccessor;

            _googleTableUpdateService = new GoogleTableUpdateService(_logger, _databaseAccessor.SubjectActivity, _databaseAccessor.Student);
            _studentService = studentService;
        }

        [HttpPost("UpdateSubjectActivityData")]
        public void UpdateSubjectActivityData(SubjectActivityEntity activity)
        {
            _databaseAccessor.SubjectActivity.Update(activity);
        }

        [HttpPost("UpdateSubjectActivityForGroup")]
        public void UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            GroupSubjectEntity groupSubjectData = _databaseAccessor.GroupSubject
                .Read()
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (groupSubjectData == null)
            {
                _logger.LogWarning($"Subject info was not found: subjectId:{subjectId}, groupId:{groupId}");
                return;
            }

            _googleTableUpdateService.UpdateSubjectActivityForGroup(groupSubjectData);
        }

        [HttpGet("login/{userId}")]
        public string Login(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _databaseAccessor.Student.Get(userId);
            return GenerateToken(userId, signingEncodingKey);
        }

        [HttpGet("loginOrCreate/{userId}")]
        public string LoginOrCreate(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _studentService.GetOrCreate(userId);
            return GenerateToken(userId, signingEncodingKey);
        }

        [HttpGet("ValidateToken")]
        public int ValidateToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(token) is JwtSecurityToken securityToken)
            {
                string stringClaimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.UserData).Value;
                return int.Parse(stringClaimValue, CultureInfo.InvariantCulture);
            }

            throw new Exception("Invalid token");
        }

        [HttpPost("register")]
        public string Register([FromBody] StudentCreateArgumentsDto arguments,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            int groupId = _databaseAccessor.StudyGroup.ReadByNamePattern(arguments.Group).Id;
            var student = new StudentEntity(arguments, groupId);

            _databaseAccessor.Student.Create(student);

            return GenerateToken(student.Id, signingEncodingKey);
        }

        private string GenerateToken(int userId, IJwtSigningEncodingKey signingEncodingKey)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData, userId.ToString(CultureInfo.InvariantCulture))
            };

            var token = new JwtSecurityToken(
                issuer: ApplicationOptions.JwtIssuer,
                audience: ApplicationOptions.JwtIssuer,
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
