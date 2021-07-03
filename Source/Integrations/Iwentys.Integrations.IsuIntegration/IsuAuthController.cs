using System.Text.Json;
using System.Threading.Tasks;
using Iwentys.Integrations.IsuIntegration.Models;
using Iwentys.Integrations.IsuIntegration.SingingLogic;
using Microsoft.AspNetCore.Mvc;
using Tef.IsuIntegrator;
using Tef.IsuIntegrator.Responses;

namespace Iwentys.Integrations.IsuIntegration
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

        //TODO: I'm no sure it is work. We missed IJwtSigningEncodingKey registration
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
                User = JsonSerializer.Serialize(userData)
            };

            return Ok(response);
        }
    }
}