using Microsoft.IdentityModel.Tokens;

namespace Iwentys.IsuIntegration.SingingLogic;

public interface IJwtSigningDecodingKey
{
    SecurityKey GetKey();
}