using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.OldShared.Auth
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}