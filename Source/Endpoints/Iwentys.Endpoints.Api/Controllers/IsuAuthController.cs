using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Iwentys.Common.Transferable;
using Iwentys.Domain.AccountManagement;
using Iwentys.Endpoints.Api.Source.Tokens;
using Iwentys.Infrastructure.Configuration.Options;
using Iwentys.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tef.IsuIntegrator;
using Tef.IsuIntegrator.Responses;

namespace Iwentys.Endpoints.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuthController : ControllerBase
    {
        private readonly IsuApiAccessor _isuApiAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtApplicationOptions _jwtApplicationOptions;

        public IsuAuthController(IUnitOfWork unitOfWork, IsuApplicationOptions isuApplicationOptions, JwtApplicationOptions jwtApplicationOptions)
        {
            _unitOfWork = unitOfWork;
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
        //TODO: do not send via query
        public ActionResult<IwentysAuthResponse> Login(int userId, [FromQuery] string password, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            UniversitySystemUserCredential universitySystemUserCredential = _unitOfWork.GetRepository<UniversitySystemUserCredential>()
                .Get()
                .FirstOrDefault(UniversitySystemUserCredential.IsCredentialMatch(userId, password));
            if (universitySystemUserCredential is not null)
                return Ok(TokenGenerator.Generate(userId, signingEncodingKey, _jwtApplicationOptions));

            //TODO: rework
            return BadRequest(new AuthenticationException("Wrong credentials"));
        }

        [HttpGet("login-with-itip/{userId}")]
        public ActionResult<IwentysAuthResponse> Login(int userId, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            return Ok(TokenGenerator.Generate(userId, signingEncodingKey, _jwtApplicationOptions));
        }
    }
}