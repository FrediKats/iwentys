using Tef.BotFramework.Settings;
using Tef.BotFramework.Telegram;

namespace Iwentys.ClientBot.Tools
{
    public class TelegramDebugSettings : IGetSettings<TelegramSettings>
    {
        public TelegramSettings GetSettings()
        {
            return new TelegramSettings();
        }
    }
}