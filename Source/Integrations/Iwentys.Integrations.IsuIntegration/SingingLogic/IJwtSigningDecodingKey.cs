using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.Api.Source.Tokens
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}