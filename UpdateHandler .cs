using System.Text.Json;
using System.Diagnostics;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramInfoBot.Utils;

namespace TelegramInfoBot;

public class UpdateHandler: IUpdateHandler
{
    public static long Id;
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Некоторые действия
        //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            Id = message.Chat.Id;

            if (message.Text.ToLower() == "/start") // вернуться
            {
                MainMenu(botClient: botClient, chatId: message.Chat.Id, cancellationToken: cancellationToken);
                return;
            }
        }
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            var message = update.Message;

            Console.OutputEncoding = Encoding.UTF8;
            string codeOfButton = update.CallbackQuery.Data;
            if (codeOfButton == "Info")
            {
                Console.WriteLine("Нажата Кнопка Info");
                string telegramMessage = "Информация о боте";
                await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            if (codeOfButton == "Pricing")
            {
                Console.WriteLine("Нажата Кнопка Pricing");
                
                PricingMenu(botClient: botClient, chatId: Id, cancellationToken: cancellationToken);
                return;
                //string telegramMessage = "Вы нажали Кнопку 2";
                // await botClient.SendTextMessageAsync(chatId: update.CallbackQuery.Message.Chat.Id, telegramMessage, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);

                //InlineKeyboardMarkup inlineKeyBoard = new InlineKeyboardMarkup(
                //    new[]
                //    {
                //            // first row
                //            new[]
                //            {
                //                // first button in row
                //                InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 1", callbackData: "Pricing1"),
                //                // second button in row
                //                InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 2", callbackData: "Pricing2"),
                //                InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 3", callbackData: "Pricing3"),
                //            },

                //    });

                //Message sentMessage = await botClient.SendTextMessageAsync(
                //    chatId: message.Chat.Id,
                //    text: "Выбери тариф",
                //    replyMarkup: inlineKeyBoard,
                //    cancellationToken: cancellationToken);

                // await botClient.EditMessageCaptionAsync(chatId: update.CallbackQuery.Message.Chat.Id, caption: telegramMessage, messageId: update.CallbackQuery.Message.MessageId);
                //await botClient.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, telegramMessage, replyMarkup: inlineKeyBoard, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
        }
        //// Only process Message updates: https://core.telegram.org/bots/api#message
        //if (update.Message is not { } message)
        //    return;
        //// Only process text messages
        //if (message.Text is not { } messageText)
        //    return;

        //var chatId = message.Chat.Id;

        //Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        //// Echo received message text
        //Message sentMessage = await botClient.SendTextMessageAsync(
        //    chatId: chatId,
        //    text: "You said:\n" + messageText,
        //    cancellationToken: cancellationToken);
    }
    //public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    //{
    //    Debug.WriteLine(JsonSerializer.Serialize(update));
    //    // Вообще, для обработки сообщений лучше подходит паттерн "Цепочка обязанностей", но для примера тут switch-case
    //    // https://refactoring.guru/ru/design-patterns/chain-of-responsibility
    //    switch (update)
    //    {
    //        case
    //        {
    //            Type: UpdateType.Message,
    //            Message: { Text: { } text, Chat: { } chat },
    //        } when text.Equals("/start", StringComparison.OrdinalIgnoreCase):
    //        {
    //            await botClient.SendTextMessageAsync(chat!, "Добро пожаловать на борт, добрый путник!", cancellationToken: cancellationToken);
    //            break;
    //        }
    //        case
    //        {
    //            Type: UpdateType.Message,
    //            Message.Chat: { } chat
    //        }:
    //        {
    //            await botClient.SendTextMessageAsync(chat!, "Привет-привет!!", cancellationToken: cancellationToken);
    //            break;
    //        }
    //    }
    //}

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
                    // first button in row
                    InlineKeyboardButton.WithUrl(text: "Ссылка", url: "https://google.com"),
                    InlineKeyboardButton.WithCallbackData("CallbackData кнопка"),
                    //InlineKeyboardButton.WithPayment("BTC"), 
                },
                new[]
                {
                    // first button in row
                    InlineKeyboardButton.WithCallbackData(text: "Старт", callbackData: "post"),
                    // second button in row
                },

            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Приветствие",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    public static async void PricingMenu(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(
            // keyboard
            new[]
            {
                // first row
                new[]
                {
                    // first button in row
                    InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 1", callbackData: "Pricing1"),
                    // second button in row
                    InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 2", callbackData: "Pricing2"),
                    InlineKeyboardButton.WithCallbackData(text: "Кнопка Тариф 3", callbackData: "Pricing3"),
                },
            });

        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Выбери тариф",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}