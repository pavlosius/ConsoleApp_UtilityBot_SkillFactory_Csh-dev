using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using UtilityBot.Configuration;
using UtilityBot.Controllers;
using UtilityBot.Services;

namespace UtilityBot
{
    static class Program
    {
        static async Task Main()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) => ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build();

            Console.WriteLine("Starting Service");
            await host.RunAsync(); // Запускаем сервис
            Console.WriteLine("Service stopped");
        }

        static void ConfigureServices(IServiceCollection services)
        {
            AppSettings appSettings = BuildAppSettings();
            services.AddSingleton(BuildAppSettings());

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddSingleton<ITextMessageHandler, TextMessageHandler>();

            // Подключаем контроллеры сообщений и кнопок
            services.AddTransient<TextMessageController>();
            services.AddTransient<InlineKeyboardController>();

            services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
            services.AddHostedService<Bot>();
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "6276665285:AAERfeWOOhlp3vni4ngN5W8SFNo3kV407qU",
            };
        }
    }
}