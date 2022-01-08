using Microsoft.IdentityModel.Tokens;

namespace Iwentys.IsuIntegration.SingingLogic;

public interface IJwtSigningEncodingKey
{
    string SigningAlgorithm { get; }

    SecurityKey GetKey();
}