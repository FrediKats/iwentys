using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Core.Auth
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}