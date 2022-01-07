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
        //TODO: fix null value
        private readonly string _jwtIssuer = null;

        public IsuAuthController(IsuApplicationOptions isuApplicationOptions)
        {
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

            IwentysAuthResponse token = TokenGenerator.Generate(userData.Id, signingEncodingKey, _jwtIssuer);
            var response = new IsuAuthResponse
            {
                Token = token.Token,
                User = JsonSerializer.Serialize(userData)
            };

            return Ok(response);
        }
    }
}