using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.Api.Source.Tokens
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}