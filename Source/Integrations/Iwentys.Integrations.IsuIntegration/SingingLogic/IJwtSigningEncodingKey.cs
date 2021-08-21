using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Integrations.IsuIntegration.SingingLogic
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}