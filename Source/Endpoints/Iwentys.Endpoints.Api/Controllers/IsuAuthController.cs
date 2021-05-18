using System.Threading.Tasks;
using Iwentys.Common.Transferable;
using Iwentys.Endpoints.Api.Source.Tokens;
using Iwentys.Infrastructure.Configuration.Options;
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
        private readonly JwtApplicationOptions _jwtApplicationOptions;

        public IsuAuthController(IsuApplicationOptions isuApplicationOptions, JwtApplicationOptions jwtApplicationOptions)
        {
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
    }
}