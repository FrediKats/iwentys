using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoint.Server.Source.Auth
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}