using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Iwentys.Core.Services;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Endpoints.Api.Tools;
using Iwentys.Endpoints.Shared;
using Iwentys.Endpoints.Shared.Auth;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Students;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tef.IsuIntegrator;
using Tef.IsuIntegrator.Responses;

namespace Iwentys.Endpoints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuthController : ControllerBase
    {
        private readonly ILogger<IsuAuthController> _logger;
        private readonly IsuApiAccessor _isuApiAccessor;
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly StudentService _studentService;

        public IsuAuthController(ILogger<IsuAuthController> logger, DatabaseAccessor databaseAccessor, StudentService studentService)
        {
            _logger = logger;
            _databaseAccessor = databaseAccessor;
            _studentService = studentService;
            _isuApiAccessor = new IsuApiAccessor(ApplicationOptions.IsuClientId, ApplicationOptions.IsuClientSecret, ApplicationOptions.IsuRedirection);
        }

        [HttpGet]
        public async Task<ActionResult<IsuAuthResponse>> Get(string code, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _logger.LogInformation($"Get code for isu auth: {code}");

            AuthorizeResponse authResponse = await _isuApiAccessor.Authorize(code);
            if (!authResponse.IsSuccess)
                return BadRequest(authResponse.ErrorResponse);

            IsuUserDataResponse userData = await _isuApiAccessor.GetUserData(authResponse.TokenResponse.AccessToken);

            IwentysAuthResponse token = TokenGenerator.Generate(userData.Id, signingEncodingKey);
            var response = new IsuAuthResponse
            {
                Token = token.Token,
                User = JsonConvert.SerializeObject(userData)
            };

            return Ok(response);
        }

        [HttpGet("login/{userId}")]
        public async Task<ActionResult<IwentysAuthResponse>> Login(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            await _databaseAccessor.Student.GetAsync(userId);
            return Ok(TokenGenerator.Generate(userId, signingEncodingKey));
        }

        [HttpGet("loginOrCreate/{userId}")]
        public async Task<ActionResult<IwentysAuthResponse>> LoginOrCreate(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            await _studentService.GetOrCreateAsync(userId);
            return Ok(TokenGenerator.Generate(userId, signingEncodingKey));
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
        public async Task<ActionResult<IwentysAuthResponse>> Register([FromServices] IJwtSigningEncodingKey signingEncodingKey, [FromBody] StudentCreateArgumentsDto arguments)
        {
            int groupId = _databaseAccessor.StudyGroup.ReadByNamePattern(new GroupName(arguments.Group)).Id;
            var student = new StudentEntity(arguments, groupId);

            await _databaseAccessor.Student.CreateAsync(student);

            return Ok(TokenGenerator.Generate(student.Id, signingEncodingKey));
        }
    }
}