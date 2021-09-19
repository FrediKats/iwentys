﻿using Microsoft.Extensions.Configuration;

namespace Iwentys.Infrastructure.Application.Options
{
    public class TokenApplicationOptions
    {
        public string GoogleServiceToken { get; set; }
        public string GithubToken { get; set; }
        public string TelegramToken { get; set; }

        public static TokenApplicationOptions Load(IConfiguration configuration)
        {
            return new TokenApplicationOptions()
            {
                GoogleServiceToken = configuration["GoogleTableCredentials"],
                GithubToken = configuration["GithubToken"],
                TelegramToken = configuration["TelegramToken"]
            };
        }
    }
}