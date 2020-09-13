using Iwentys.Core;
using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.Api.Tools
{
    public class TelegramDebugSettings : IGetSettings<TelegramSettings>
    {
        private readonly TelegramSettings _settings;

        public TelegramDebugSettings()
        {
            _settings = new TelegramSettings(ApplicationOptions.TelegramToken);
        }

        public TelegramSettings GetSettings()
        {
            return _settings;
        }
    }
}