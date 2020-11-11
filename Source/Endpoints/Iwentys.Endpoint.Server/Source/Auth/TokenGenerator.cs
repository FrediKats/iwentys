using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Iwentys.Models.Transferable;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.OldShared.Auth
{
    public static class TokenGenerator
    {
        public static IwentysAuthResponse Generate(int userId, IJwtSigningEncodingKey signingEncodingKey)
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

            var jwt = new JwtSecurityTokenHandler();
            return new IwentysAuthResponse
            {
                Token = jwt.WriteToken(token)
            };
        }
    }
}
//TODO: fix