using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoint.Server.Source.Tokens
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}