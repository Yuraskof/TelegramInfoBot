using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using TelegramInfoBot.Utils;
using LiteDB;
using TelegramInfoBot;

var botClient = new TelegramBotClient(FileUtils.BotToken.Token);
var database = new LiteDatabase("preferences.db");


var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cts.Cancel(); // Чтобы отловить нажатие ctrl+C и всякие sigterm, sigkill, etc

var handler = new UpdateHandler();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(updateHandler: handler.HandleUpdateAsync, pollingErrorHandler: handler.HandlePollingErrorAsync,
    receiverOptions: receiverOptions, cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

Console.ReadLine(); 

