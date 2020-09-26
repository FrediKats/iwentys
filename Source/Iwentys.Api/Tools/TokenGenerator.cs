using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Iwentys.Core;
using Iwentys.Core.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Api.Tools
{
    public static class TokenGenerator
    {
        public static string Generate(int userId, IJwtSigningEncodingKey signingEncodingKey)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.UserData, userId.ToString(CultureInfo.InvariantCulture))
            };

            var token = new JwtSecurityToken(
                issuer: ApplicationOptions.JwtIssuer,
                audience: ApplicationOptions.JwtIssuer,
                claims: claims,
                signingCredentials: new SigningCredentials(
                    signingEncodingKey.GetKey(),
                    signingEncodingKey.SigningAlgorithm)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}