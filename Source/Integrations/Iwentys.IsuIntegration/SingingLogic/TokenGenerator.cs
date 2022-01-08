using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Iwentys.IsuIntegration.Models;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.IsuIntegration.SingingLogic;

public static class TokenGenerator
{
    public static IwentysAuthResponse Generate(int userId, IJwtSigningEncodingKey signingEncodingKey, string jwtIssuer)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.UserData, userId.ToString(CultureInfo.InvariantCulture))
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtIssuer,
            claims: claims,
            signingCredentials: new SigningCredentials(
                signingEncodingKey.GetKey(),
                signingEncodingKey.SigningAlgorithm)
        );

        var jwt = new JwtSecurityTokenHandler();
        return new IwentysAuthResponse
        {
            Token = jwt.WriteToken(token)
        };
    }
}