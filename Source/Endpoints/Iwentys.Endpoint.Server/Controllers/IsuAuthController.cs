using System;
using System.Threading.Tasks;
using Iwentys.Common.Transferable;
using Iwentys.Endpoint.Controllers.Tools;
using Iwentys.Endpoint.Server.Source.Options;
using Iwentys.Endpoint.Server.Source.Tokens;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tef.IsuIntegrator;
using Tef.IsuIntegrator.Responses;

namespace Iwentys.Endpoint.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuthController : ControllerBase
    {
        private readonly IsuApiAccessor _isuApiAccessor;
        private readonly StudentService _studentService;
        private readonly JwtApplicationOptions _jwtApplicationOptions;

        public IsuAuthController(StudentService studentService, IsuApplicationOptions isuApplicationOptions, JwtApplicationOptions jwtApplicationOptions)
        {
            _studentService = studentService;
            _jwtApplicationOptions = jwtApplicationOptions;
            _isuApiAccessor = new IsuApiAccessor(isuApplicationOptions.IsuClientId, isuApplicationOptions.IsuClientSecret, isuApplicationOptions.IsuRedirection);
        }

        [HttpGet]
        public async Task<ActionResult<IsuAuthResponse>> Get(string code, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            AuthorizeResponse authResponse = await _isuApiAccessor.Authorize(code);
            if (!authResponse.IsSuccess)
                return BadRequest(authResponse.ErrorResponse);

            IsuUserDataResponse userData = await _isuApiAccessor.GetUserData(authResponse.TokenResponse.AccessToken);

            IwentysAuthResponse token = TokenGenerator.Generate(userData.Id, signingEncodingKey, _jwtApplicationOptions);
            var response = new IsuAuthResponse
            {
                Token = token.Token,
                User = JsonConvert.SerializeObject(userData)
            };

            return Ok(response);
        }

        [HttpGet("login/{userId}")]
        public ActionResult<IwentysAuthResponse> Login(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            return Ok(TokenGenerator.Generate(userId, signingEncodingKey, _jwtApplicationOptions));
        }

        [HttpGet("loginOrCreate/{userId}")]
        public async Task<ActionResult<IwentysAuthResponse>> LoginOrCreate(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            await _studentService.GetOrCreateAsync(userId);
            return Ok(TokenGenerator.Generate(userId, signingEncodingKey, _jwtApplicationOptions));
        }

        [HttpGet("ValidateToken")]
        public int ValidateToken()
        {
            AuthorizedUser tryAuthWithToken = this.TryAuthWithTokenOrDefault(-1);
            if (tryAuthWithToken.Id == -1)
                throw new Exception("Invalid token");
            return tryAuthWithToken.Id;
        }
    }
}