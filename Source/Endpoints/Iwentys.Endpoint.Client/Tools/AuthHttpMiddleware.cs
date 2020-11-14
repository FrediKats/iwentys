using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwentys.Endpoint.Client.Tools
{
    public class AuthHttpMiddleware : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}