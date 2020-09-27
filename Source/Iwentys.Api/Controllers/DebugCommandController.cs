using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Iwentys.Api.Tools;
using Iwentys.Core.Auth;
using Iwentys.Core.GoogleTableIntegration;
using Iwentys.Core.GoogleTableIntegration.Marks;
using Iwentys.Core.GoogleTableIntegration.TeacherInfoParse;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly MarkGoogleTableUpdateService _markGoogleTableUpdateService;
        private readonly ILogger<DebugCommandController> _logger;
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IStudentService _studentService;

        public DebugCommandController(ILogger<DebugCommandController> logger, DatabaseAccessor databaseAccessor, IStudentService studentService)
        {
            _logger = logger;
            _databaseAccessor = databaseAccessor;

            _markGoogleTableUpdateService = new MarkGoogleTableUpdateService(_logger, _databaseAccessor.SubjectActivity, _databaseAccessor.Student);
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

            _markGoogleTableUpdateService.UpdateSubjectActivityForGroup(groupSubjectData);
        }

        [HttpGet("login/{userId}")]
        public ActionResult<IwentysAuthResponse> Login(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _databaseAccessor.Student.Get(userId);
            return TokenGenerator.Generate(userId, signingEncodingKey);
        }

        [HttpGet("loginOrCreate/{userId}")]
        public ActionResult<IwentysAuthResponse> LoginOrCreate(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _studentService.GetOrCreate(userId);
            return TokenGenerator.Generate(userId, signingEncodingKey);
        }

        [HttpGet("ValidateToken")]
        public int ValidateToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (token.StartsWith("Bearer "))
                token = token.Remove(0, "Bearer ".Length);

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenHandler.ReadToken(token) is JwtSecurityToken securityToken)
            {
                string stringClaimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.UserData).Value;
                return int.Parse(stringClaimValue, CultureInfo.InvariantCulture);
            }

            throw new Exception("Invalid token");
        }

        [HttpPost("register")]
        public ActionResult<IwentysAuthResponse> Register([FromBody] StudentCreateArgumentsDto arguments,
            [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            int groupId = _databaseAccessor.StudyGroup.ReadByNamePattern(arguments.Group).Id;
            var student = new StudentEntity(arguments, groupId);

            _databaseAccessor.Student.Create(student);

            return TokenGenerator.Generate(student.Id, signingEncodingKey);
        }

        [HttpGet("teachers")]
        public ActionResult<List<SubjectTeacherInfo>> LoadTeachers([FromQuery] string tableId, [FromQuery] string range)
        {
            var tableParser = TableParser.Create(_logger);
            var subjectTeacherParser = new SubjectTeacherParser(tableId, range);
            List<SubjectTeacherInfo> result = tableParser.Execute(subjectTeacherParser);
            return Ok(result);
        }
    }
}
