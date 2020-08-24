using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.Bot.Tools
{
    public class TelegramDebugSettings : IGetSettings<TelegramSettings>
    {
        public TelegramSettings GetSettings()
        {
            return new TelegramSettings();
        }
    }
}