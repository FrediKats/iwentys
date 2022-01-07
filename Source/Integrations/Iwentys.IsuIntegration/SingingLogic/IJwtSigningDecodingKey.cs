using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Integrations.IsuIntegration.SingingLogic
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}