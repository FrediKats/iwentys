using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.Shared.Auth
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}