using System.Collections.Generic;
using System.Net.Http;
using Iwentys.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsuAuthController : ControllerBase
    {
        private readonly ILogger<IsuAuthController> _logger;

        public IsuAuthController(ILogger<IsuAuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<string> Get(string code)
        {
            _logger.LogInformation($"Get code for isu auth: {code}");

            using HttpClient client = new HttpClient();
            var parameters = new Dictionary<string, string>
            {
                {"grant_type", "authorization_code"},
                {"client_id", ApplicationOptions.IsuClientId},
                {"client_secret", ApplicationOptions.IsuClientSecret},
                {"redirect_uri", ApplicationOptions.IsuRedirection},
                {"code", code}
            };

            using var content = new FormUrlEncodedContent(parameters);
            HttpResponseMessage result = client.PostAsync(ApplicationOptions.IsuAuthUrl, content).Result;
            return Ok(result.Content.ReadAsStringAsync().Result);
        }
    }
}