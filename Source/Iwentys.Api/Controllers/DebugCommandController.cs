using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Iwentys.Core.Auth;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly GoogleTableUpdateService _googleTableUpdateService;
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IStudentService _studentService;

        public DebugCommandController(DatabaseAccessor databaseAccessor, IConfiguration configuration, IStudentService studentService)
        {
            _databaseAccessor = databaseAccessor;

            _googleTableUpdateService = new GoogleTableUpdateService(_databaseAccessor.SubjectActivity, configuration);
            _studentService = studentService;
        }

        [HttpPost("UpdateSubjectActivityData")]
        public void UpdateSubjectActivityData(SubjectActivity activity)
        {
            _databaseAccessor.SubjectActivity.Update(activity);
        }

        [HttpPost("UpdateSubjectActivityForGroup")]
        public void UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            SubjectForGroup subjectData = _databaseAccessor.SubjectForGroup
                .Read()
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (subjectData == null)
            {
                // TODO: Some logs
                return;
            }
            
            _googleTableUpdateService.UpdateSubjectActivityForGroup(subjectData);
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

        [HttpPost("register")]
        public string Register([FromBody] StudentCreateArgumentsDto arguments,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            var student = new Student
            {
                Id = arguments.Id,
                FirstName = arguments.FirstName,
                MiddleName = arguments.MiddleName,
                SecondName = arguments.SecondName,
                Role = arguments.Role,
                Group = arguments.Group,
                GithubUsername = arguments.GithubUsername,
                CreationTime = DateTime.UtcNow,
                LastOnlineTime = DateTime.UtcNow,
                BarsPoints = arguments.BarsPoints,
                GuildLeftTime = DateTime.MinValue.ToUniversalTime()
            };

            _databaseAccessor.Student.Create(student);

            return GenerateToken(student.Id, signingEncodingKey);
        }

        private string GenerateToken(int userId, IJwtSigningEncodingKey signingEncodingKey)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData, userId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: "Iwentys",
                audience: "IwentysWeb",
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
