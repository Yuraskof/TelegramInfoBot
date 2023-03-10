using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInfoBot.Models;
using TelegramInfoBot.Utils;


namespace TelegramInfoBot;

public class UpdateHandler: IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.OutputEncoding = Encoding.UTF8;

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            
            if (message.Text.ToLower() == "/start" || message.Text.ToLower() == "/command1")
            {
                Console.WriteLine($"При первом запуске бота, скопировать Chat Id в файл BotInfo.json для получения сообщений от остальных ботов");
                Console.WriteLine($"Chat Id = {message.Chat.Id}");
                Console.WriteLine($"Username = {message.From.Username}, {message.From.FirstName} {message.From.LastName}");

                DatabaseUtil.SaveBotParam(message.From.Username, UserFields.FirstLastName, $"{message.From.FirstName} {message.From.LastName}");
                
                MainMenu(botClient: botClient, chatId: message.Chat.Id, cancellationToken: cancellationToken, "Приветствие");
                return;
            }
        }
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            string codeOfButton = update.CallbackQuery.Data;

            if (codeOfButton.Contains("MainMenu"))
            {
                SelectFromMainMenu(botClient, update, cancellationToken, codeOfButton);
                return;
            }
            
            if (codeOfButton.Contains("Pricing")) 
            {
                SelectPricing(botClient, update, cancellationToken, codeOfButton);
                return;
            }

            if (codeOfButton.Contains("Strategy")) 
            {
                SelectStrategy(botClient, update, cancellationToken, codeOfButton);
                return;
            }

            if (codeOfButton.Contains("Payment"))
            {
                SelectPayment(botClient, update, cancellationToken, codeOfButton);
                return;
            }

            if (codeOfButton.Contains("Deposit"))
            {
                SelectDeposit(botClient, update, cancellationToken, codeOfButton);
                return;
            }
        }
    }

    private void SelectDeposit(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string codeOfButton)
    {
        if (codeOfButton == "Deposit1")
        {
            Console.WriteLine("Пользователь выбрал Deposit1");

            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Deposit, codeOfButton);

            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбран Deposit1");
            return;
        }
        if (codeOfButton == "Deposit2")
        {
            Console.WriteLine("Пользователь выбрал Deposit2");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Deposit, codeOfButton);
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбран Deposit2");
            return;

        }
        if (codeOfButton == "Deposit3")
        {
            Console.WriteLine("Пользователь выбрал Deposit3");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Deposit, codeOfButton);

            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбран Deposit3");
            return;
        }
    }

    private async void SelectPayment(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string codeOfButton)
    {
        if (codeOfButton == "PaymentInfo")
        {
            Console.WriteLine("Нажата Кнопка Информация о способах оплаты");
            string telegramMessage = "Информация о способах оплаты";
            await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.From.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            return;
        }
        if (codeOfButton == "TON Payment")
        {
            Console.WriteLine("Пользователь выбрал оплату в TON");

            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.PaymentType, codeOfButton);

            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбрана оплата в TON");
            return;
        }
        if (codeOfButton == "Fiat Payment")
        {
            Console.WriteLine("Пользователь выбрал оплату фиатом");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.PaymentType, codeOfButton);
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбрана оплата фиатом");
            return;

        }
        if (codeOfButton == "Crypto Payment")
        {
            Console.WriteLine("Пользователь выбрал оплату криптой");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.PaymentType, codeOfButton);

            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбрана оплата криптой");
            return;
        }
    }

    private void SelectStrategy(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string codeOfButton)
    {
        if (codeOfButton == "Aggressive Strategy")
        {
            Console.WriteLine("Пользователь выбрал Aggressive Strategy");

            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Strategy, codeOfButton);

            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбрана стратегия Aggressive");
            return;
        }
        if (codeOfButton == "Medium Strategy")
        {
            Console.WriteLine("Пользователь выбрал Medium Strategy");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Strategy, codeOfButton);
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбрана стратегия Medium");
            return;

        }
        if (codeOfButton == "Lite Strategy")
        {
            Console.WriteLine("Пользователь выбрал Lite Strategy");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Strategy, codeOfButton);

            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбрана стратегия Lite");
            return;
        }
    }

    private async void SelectFromMainMenu(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string codeOfButton)
    {
        if (codeOfButton == "InfoButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка Info");
            string telegramMessage = "Информация о боте";
            await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.From.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            return;
        }
        if (codeOfButton == "PricingButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка Выбор тарифа");

            PricingMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
            return;

            // await botClient.EditMessageCaptionAsync(chatId: update.CallbackQuery.Message.Chat.Id, caption: telegramMessage, messageId: update.CallbackQuery.Message.MessageId);
            //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: inlineKeyBoard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        if (codeOfButton == "MainMenuButton")
        {
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Главное меню");
            return;
        }
        if (codeOfButton == "PaymentButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка Выбор способа оплаты");

            PaymentMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
            return;
        }
        if (codeOfButton == "DepositButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка выбор депозита");

            DepositMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
            return;
        }
        if (codeOfButton == "StrategyButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка выбор стратегии");

            StrategyMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
            return;
        }
        if (codeOfButton == "StartButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка Start");

            try
            {
                string telegramMessage = DatabaseUtil.LoadBotParam(update.CallbackQuery.From.Username);
                await botClient.SendTextMessageAsync(chatId: FileUtils.BotInfo.ChatId, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
               
                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: "Бот успешно запущен!",
                    cancellationToken: cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine("Пользователь заполнил не все поля");

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: update.CallbackQuery.From.Id,
                    text: e.Message,
                    cancellationToken: cancellationToken);
            }

            return;

            //StrategyMenu(botClient: botClient, chatId: update.CallbackQuery.From.Id, cancellationToken: cancellationToken);
            //return;
        }
        if (codeOfButton == "SettingsButtonMainMenu")
        {
            Console.WriteLine("Нажата Кнопка Посмотреть текущие настройки");

            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: update.CallbackQuery.From.Id,
                text: DatabaseUtil.LoadBotParamForUser(update.CallbackQuery.From.Username),
                cancellationToken: cancellationToken);
        }
    }

    private async void SelectPricing(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string codeOfButton)
    {
        if (codeOfButton == "PricingInfo")
        {
            Console.WriteLine("Нажата Кнопка PricingInfo");
            string telegramMessage = "Информация о тарифах";
            await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.From.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            return;
        }
        if (codeOfButton == "Pricing1")
        {
            Console.WriteLine("Пользователь выбрал тариф 1");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Pricing, codeOfButton);
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбран тариф 1");
            return;
        }
        if (codeOfButton == "Pricing2")
        {
            Console.WriteLine("Пользователь выбрал тариф 2");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Pricing, codeOfButton);
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбран тариф 2");
            return;

        }
        if (codeOfButton == "Pricing3")
        {
            Console.WriteLine("Пользователь выбрал тариф 3");
            DatabaseUtil.SaveBotParam(update.CallbackQuery.From.Username, UserFields.Pricing, codeOfButton);
            MainMenu(botClient: botClient, update.CallbackQuery.From.Id, cancellationToken: cancellationToken, "Выбран тариф 3");
            return;
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

    public static async void MainMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken, string textMessage)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            // keyboard
            new[]
            {
                // first row
                new[]
                {
                    // first button in row
                    InlineKeyboardButton.WithCallbackData(text: "Информация о боте", callbackData: "InfoButtonMainMenu"),
                },
                // second row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Выбор тарифа", callbackData: "PricingButtonMainMenu"),
                    InlineKeyboardButton.WithCallbackData(text: "Выбор способа оплаты", callbackData: "PaymentButtonMainMenu"),
                    
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Выбор депозита", callbackData: "DepositButtonMainMenu"),
                    InlineKeyboardButton.WithCallbackData("Выбор стратегии", callbackData: "StrategyButtonMainMenu"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Текущие настройки", callbackData: "SettingsButtonMainMenu"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Старт", callbackData: "StartButtonMainMenu"),
                },

            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: textMessage,
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
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenuButton"),
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
                    InlineKeyboardButton.WithCallbackData(text: "TON", callbackData: "TON Payment"),
                    InlineKeyboardButton.WithCallbackData(text: "Фиат", callbackData: "Fiat Payment"),
                    InlineKeyboardButton.WithCallbackData(text: "Крипта", callbackData: "Crypto Payment"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenuButton"),
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
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenuButton"),
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
                    InlineKeyboardButton.WithCallbackData(text: "Aggressive", callbackData: "Aggressive Strategy"),
                    InlineKeyboardButton.WithCallbackData(text: "Medium", callbackData: "Medium Strategy"),
                    InlineKeyboardButton.WithCallbackData(text: "Lite", callbackData: "Lite Strategy"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Показать главное меню", callbackData: "MainMenuButton"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери стратегию",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}