using Telegram.Bots.Types;

namespace UtilityBot.Services
{
    internal interface ITextMessageHandler
    {
        string Process(string message, string mode);
    }
}
