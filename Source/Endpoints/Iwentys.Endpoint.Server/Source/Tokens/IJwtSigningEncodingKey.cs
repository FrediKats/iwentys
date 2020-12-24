using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoint.Server.Source.Tokens
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}