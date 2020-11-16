using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoint.Server.Source.Auth
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}