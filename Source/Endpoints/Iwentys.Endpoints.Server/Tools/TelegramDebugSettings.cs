using Iwentys.Endpoints.OldShared;
using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.Endpoints.OldServer.Tools
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