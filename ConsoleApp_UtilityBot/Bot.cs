using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bots.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.Extensions.Hosting;
using UtilityBot.Controllers;

namespace UtilityBot
{
    internal class Bot : BackgroundService
    {
        /// <summary>
        /// объект, отвеающий за отправку сообщений клиенту
        /// </summary>
        private ITelegramBotClient _telegramClient;

        // Контроллеры различных видов сообщений
        private InlineKeyboardController _inlineKeyboardController;
        private TextMessageController _textMessageController;

        public Bot(
            ITelegramBotClient telegramClient,
            InlineKeyboardController inlineKeyboardController,
            TextMessageController textMessageController)
        {
            _telegramClient = telegramClient;
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // receive all update types
                cancellationToken: stoppingToken);

            Console.WriteLine("Bot started");
        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
            {
                Console.WriteLine($"Нажатие на кнопку");
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken);
                        return;
                    default: // unsupported message
                        await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Данный тип сообщений не поддерживается. Пожалуйста отправьте текст.", 
                            cancellationToken: cancellationToken);
                        return;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Waiting 10 seconds before retry");
            Thread.Sleep(10000);
            return Task.CompletedTask;
        }
    }
}
