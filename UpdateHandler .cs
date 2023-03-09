using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInfoBot.Utils;
using static System.Net.WebRequestMethods;


namespace TelegramInfoBot;

public class UpdateHandler: IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            

            if (message.Text.ToLower() == "/start" || message.Text.ToLower() == "/command1") // TODO вернуться, info from files,
                                                                                             // после каждого выбора сохранять в переменные в бд для послед отправки
                                                                                             // перед Старт - проверка заполненности всех пунктов
            {
                Console.WriteLine($"При первом запуске бота, скопировать Chat Id в файл (fileName) для получения сообщений от остальных ботов");
                Console.WriteLine($"Chat Id = {message.Chat.Id}");
                Console.WriteLine($"Username = {message.From.Username}");
                MainMenu(botClient: botClient, chatId: message.Chat.Id, cancellationToken: cancellationToken);
                return;
            }
        }
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            Console.OutputEncoding = Encoding.UTF8;

            string codeOfButton = update.CallbackQuery.Data;

            if (codeOfButton == "Info")
            {
                Console.WriteLine("Нажата Кнопка Info");
                string telegramMessage = "Информация о боте";
                await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.From.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                return;
            }
            if (codeOfButton == "Pricing")
            {
                Console.WriteLine("Нажата Кнопка Pricing");
                
                PricingMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
                return;
                
                // await botClient.EditMessageCaptionAsync(chatId: update.CallbackQuery.Message.Chat.Id, caption: telegramMessage, messageId: update.CallbackQuery.Message.MessageId);
                //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: inlineKeyBoard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            if (codeOfButton == "PricingInfo")
            {
                Console.WriteLine("Нажата Кнопка PricingInfo");
                string telegramMessage = "Информация о тарифах";
                await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.From.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                return;
            }
            if (codeOfButton == "MainMenu")
            {
                MainMenuSimple(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
                return;
            }
            if (codeOfButton == "Payment")
            {
                Console.WriteLine("Нажата Кнопка Payment");

                PaymentMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
                return;
            }
            if (codeOfButton == "Deposit")
            {
                Console.WriteLine("Нажата Кнопка Deposit");

                DepositMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
                return;
            }
            if (codeOfButton == "Strategy")
            {
                Console.WriteLine("Нажата Кнопка Strategy");

                StrategyMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
                return;
            }
            if (codeOfButton == "Start")
            {
                Console.WriteLine("Нажата Кнопка Start");
                string telegramMessage = "Start send";
                await botClient.SendTextMessageAsync(chatId: FileUtils.BotInfo.ChatId, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                return;
                //StrategyMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
                //return;
            }
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    public static async void MainMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            // keyboard
            new[]
            {
                // first row
                new[]
                {
                    // first button in row
                    InlineKeyboardButton.WithCallbackData(text: "Информация о боте", callbackData: "Info"),
                },
                // second row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Выбор тарифа", callbackData: "Pricing"),
                    InlineKeyboardButton.WithCallbackData(text: "Выбор способа оплаты", callbackData: "Payment"),
                    
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Выбор депозита", callbackData: "Deposit"),
                    InlineKeyboardButton.WithCallbackData("Выбор стратегии", callbackData: "Strategy"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Старт", callbackData: "Start"),
                },

            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Приветствие",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public static async void MainMenuSimple(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Информация о боте", callbackData: "Info"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Выбор тарифа", callbackData: "Pricing"),
                    InlineKeyboardButton.WithCallbackData(text: "Выбор способа оплаты", callbackData: "Payment"), 
                    
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Выбор депозита", callbackData: "Deposit"),
                    InlineKeyboardButton.WithCallbackData("Выбор стратегии", callbackData: "Strategy"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Старт", callbackData: "Start"),
                },

            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Главное меню",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);

    }

    public static async void PricingMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            
            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Информация о тарифах", callbackData: "PricingInfo"),
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 1", callbackData: "Pricing1"),
                    InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 2", callbackData: "Pricing2"),
                    InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 3", callbackData: "Pricing3"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenu"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери тариф",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public static async void PaymentMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(

            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Информация о способах оплаты", callbackData: "PaymentInfo"),
                },

                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "TON", callbackData: "Payment1"),
                    InlineKeyboardButton.WithCallbackData(text: "Фиат", callbackData: "Payment2"),
                    InlineKeyboardButton.WithCallbackData(text: "Крипта", callbackData: "Payment3"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenu"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери способ оплаты",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public static async void DepositMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(

            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Dep1", callbackData: "Deposit1"),
                    InlineKeyboardButton.WithCallbackData(text: "Dep2", callbackData: "Deposit2"),
                    InlineKeyboardButton.WithCallbackData(text: "Dep3", callbackData: "Deposit3"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenu"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери депозит",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public static async void StrategyMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(

            new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Aggressive", callbackData: "Strategy1"),
                    InlineKeyboardButton.WithCallbackData(text: "Medium", callbackData: "Strategy2"),
                    InlineKeyboardButton.WithCallbackData(text: "Lite", callbackData: "Strategy3"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenu"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери стратегию",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}