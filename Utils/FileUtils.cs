using System.Text;
using TelegramInfoBot.Constants;

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