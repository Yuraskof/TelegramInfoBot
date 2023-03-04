using Newtonsoft.Json;

namespace TelegramInfoBot.Utils
{
    public static class JsonUtils
    {
        public static T ReadJsonDataFromPath<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(FileUtils.ReadFile(path));
        }
    }
}
