using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Models;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    internal class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;

        private readonly IStorage _memoryStorage;
        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).ProcessMode = callbackQuery.Data;

            // Генерим информационное сообщение
            string processModeText = callbackQuery.Data switch
            {
                "processMode1" => " Подсчет количества символов",
                "processMode2" => " Вычисление суммы чисел",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбран режим - {processModeText}</b>",
                cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }
}
