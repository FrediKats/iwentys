using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Endpoints.Shared.Auth
{
    public class SigningSymmetricKey : IJwtSigningEncodingKey, IJwtSigningDecodingKey
    {
        private readonly SymmetricSecurityKey _securityKey;

        public SigningSymmetricKey(string key)
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public String SigningAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

        public SecurityKey GetKey() => _securityKey;
    }
}