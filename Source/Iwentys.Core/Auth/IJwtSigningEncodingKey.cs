using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Core.Auth
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}