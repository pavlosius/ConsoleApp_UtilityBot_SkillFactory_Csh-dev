using UtilityBot.Models;

namespace UtilityBot.Services
{
    internal interface IStorage
    {
        /// <summary>
        /// Получение сессии пользователя по идентификатору
        /// </summary>
        Session GetSession(long chatId);
    }
}
