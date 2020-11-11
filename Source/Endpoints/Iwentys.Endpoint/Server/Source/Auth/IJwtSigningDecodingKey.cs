using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.OldShared.Auth
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}