using LiteDB;
using TelegramInfoBot.Models;

namespace TelegramInfoBot.Utils
{
    public static class DatabaseUtil
    {
        public static void SaveBotParam(string userName, UserFields field, string value)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../UserInfo.db; connection = shared"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<User>("BotUsers");

                // Create your new customer instance
                var currentUser = col.FindOne(x => x.Username == userName);

                if (currentUser == null)
                {
                    currentUser = new User();
                    currentUser.Username = userName;
                }

                switch (field)
                {
                    case UserFields.FirstLastName:
                        currentUser.FirstLastName = value;
                        break;
                    case UserFields.Deposit:
                        currentUser.Deposit = value;
                        break;
                    case UserFields.PaymentType:
                        currentUser.PaymentType = value;
                        break;
                    case UserFields.Pricing:
                        currentUser.Pricing = value;
                        break;
                    case UserFields.Strategy:
                        currentUser.Strategy = value;
                        break;
                }

                col.Upsert(currentUser);
                db.Commit();
            }
        }

        public static string LoadBotParam(string userName)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../UserInfo.db; connection = shared"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<User>("BotUsers");

                // Create your new customer instance
                var currentUser = col.FindOne(x => x.Username == userName);

                if (currentUser == null)
                {
                    return null; // message?
                }

                string errorMessage = null;

                if (currentUser.Pricing == null)
                {
                    errorMessage = "Необходимо выбрать тариф \n";
                }
                if (currentUser.Deposit == null)
                {
                    errorMessage = $"{errorMessage}Необходимо выбрать депозит \n";
                }
                if (currentUser.PaymentType == null)
                {
                    errorMessage = $"{errorMessage}Необходимо выбрать способ оплаты \n";
                }
                if (currentUser.Strategy == null)
                {
                    errorMessage = $"{errorMessage}Необходимо выбрать стратегию";
                }

                if (errorMessage != null)
                {
                    throw new Exception(errorMessage);
                }

                return
                    $"Пользователь {currentUser.FirstLastName} @{currentUser.Username},\nТариф - {currentUser.Pricing},\nСтратегия - {currentUser.Strategy},\nДепозит - {currentUser.Deposit},\nСпособ оплаты - {currentUser.PaymentType}";
            }
        }

        public static string LoadBotParamForUser(string userName)
        {
            using (var db = new LiteDatabase(@"Filename = ../../../UserInfo.db; connection = shared"))
            {
                // Get a collection (or create, if doesn't exist)
                var col = db.GetCollection<User>("BotUsers");

                // Create your new customer instance
                var currentUser = col.FindOne(x => x.Username == userName);

                if (currentUser == null)
                {
                    return null; 
                }

                
                var message = currentUser.Pricing == null ? "Tариф не выбран,\n" : $"Тариф  - {currentUser.Pricing},\n";

                message = currentUser.Strategy == null ? $"{message}Стратегия не выбрана,\n" : $"{message}Стратегия  - {currentUser.Strategy},\n";

                message = currentUser.Deposit == null? $"{message}Депозит не выбран,\n" : $"{message}Депозит  - {currentUser.Deposit},\n";

                message = currentUser.PaymentType == null ? $"{message}Способ оплаты не выбран\n" : $"{message}Способ оплаты  - {currentUser.PaymentType}\n";


                return message;
            }
        }
    }
}
