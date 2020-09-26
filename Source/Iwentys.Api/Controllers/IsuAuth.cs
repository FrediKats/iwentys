using Iwentys.Api.Tools;
using Iwentys.Core;
using Iwentys.Core.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tef.IsuIntegrator;
using Tef.IsuIntegrator.Responses;

namespace Iwentys.Api.Controllers
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
        public ActionResult<string> Get(string code, [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        {
            _logger.LogInformation($"Get code for isu auth: {code}");
            AuthorizeResponse authResponse = _isuApiAccessor.Authorize(code).Result;
            if (!authResponse.IsSuccess)
                return BadRequest(authResponse.ErrorResponse);

            IsuUserDataResponse userData = _isuApiAccessor.GetUserData(authResponse.TokenResponse.AccessToken).Result;
            string token = TokenGenerator.Generate(userData.Id, signingEncodingKey);
            return Ok(token);
        }
    }
}