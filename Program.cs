using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using TelegramInfoBot.Utils;
using TelegramInfoBot;

var botClient = new TelegramBotClient(FileUtils.BotInfo.Token);

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

