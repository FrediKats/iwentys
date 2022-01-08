using Microsoft.Extensions.Configuration;

namespace Iwentys.WebService.AuthComponents;

public class JwtApplicationOptions
{
    public string SigningSecurityKey { get; set; }
    public string JwtIssuer { get; set; }

    public static JwtApplicationOptions Load(IConfiguration configuration)
    {
        return new JwtApplicationOptions()
        {
            SigningSecurityKey = configuration["jwt:SigningSecurityKey"],
            JwtIssuer = configuration["jwt:issuer"],
        };
    }
}