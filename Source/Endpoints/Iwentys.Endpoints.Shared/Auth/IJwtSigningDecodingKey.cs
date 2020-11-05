using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.Shared.Auth
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}