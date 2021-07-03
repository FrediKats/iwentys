using System;
using System.Text;
using Iwentys.Endpoints.Api.Source.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Iwentys.Integrations.IsuIntegration.SingingLogic
{
    public class SigningSymmetricKey : IJwtSigningEncodingKey, IJwtSigningDecodingKey
    {
        private readonly SymmetricSecurityKey _securityKey;

        public SigningSymmetricKey(string key)
        {
            if (key is null)
                throw new ArgumentException("SigningSymmetricKey is not set. Check app settings.");

            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public string SigningAlgorithm { get; } = SecurityAlgorithms.HmacSha256;

        public SecurityKey GetKey() => _securityKey;
    }
}