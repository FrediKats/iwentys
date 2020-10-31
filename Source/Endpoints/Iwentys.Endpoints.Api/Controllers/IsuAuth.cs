using System.Threading.Tasks;
using Iwentys.Endpoints.Api.Tools;
using Iwentys.Endpoints.Shared;
using Iwentys.Endpoints.Shared.Auth;
using Iwentys.Models.Transferable;
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

        public IsuAuthController(ILogger<IsuAuthController> logger)
        {
            _logger = logger;
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
    }
}