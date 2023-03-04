using System.Text;
using TelegramInfoBot.Constants;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using TelegramInfoBot.Utils;
using LiteDB;

namespace TelegramInfoBot.Utils
{
    public static class FileUtils
    {
        public static readonly BotToken BotToken = JsonUtils.ReadJsonDataFromPath<BotToken>(FileConstants.PathToBotToken);

        public static string ReadFile(string path)
        {
            StreamReader sr = new(path, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}