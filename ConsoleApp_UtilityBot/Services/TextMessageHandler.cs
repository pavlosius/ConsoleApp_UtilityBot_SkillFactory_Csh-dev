using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bots.Types;
using UtilityBot.Configuration;

namespace UtilityBot.Services
{
    internal class TextMessageHandler : ITextMessageHandler
    {
        private readonly AppSettings _appSettings;
        private readonly ITelegramBotClient _telegramBotClient;
        public TextMessageHandler(ITelegramBotClient telegramBotClient, AppSettings appSettings)
        {
            _appSettings = appSettings;
            _telegramBotClient = telegramBotClient;
        }
        public string Process(string message, string ProcessMode)
        {
            string result = string.Empty;

            switch (ProcessMode)
            {
                case "processMode1":
                    result = $"Длина сообщения: {message.Length.ToString()} знаков";
                    break;
                case "processMode2":
                    string[] values = message.Split(" ");
                    double sum = 0;
                    foreach (string value in values)
                    {
                        bool isConverted = int.TryParse(value, out var number);
                        if (isConverted)
                            sum = sum + number;
                    }
                    result = $"Сумма чисел равна {sum.ToString()}";
                    break;
                default:
                    result = "Непредвиденная ошибка";
                    break;
            }
            return result;
        }
    }
}
