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

                return
                    $"{currentUser.Username}, {currentUser.FirstLastName}, {currentUser.Pricing}, {currentUser.Strategy}, {currentUser.Deposit}, {currentUser.PaymentType}";
            }
        }
    }
}
