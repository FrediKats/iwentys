using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.Endpoints.ClientBot.Tools
{
    public class TelegramDebugSettings : IGetSettings<TelegramSettings>
    {
        private readonly TelegramSettings _settings;

        public TelegramDebugSettings(string token)
        {
            _settings = new TelegramSettings(token);
        }

        public TelegramSettings GetSettings()
        {
            return _settings;
        }
    }
}