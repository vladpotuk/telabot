using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

class Program
{
    private static ITelegramBotClient? botClient;

    static async Task Main(string[] args)
    {
        botClient = new TelegramBotClient("7471659810:AAF3rNSGu0NQoxeBE4I2yxOZDKpn2vP5IOM");

        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");

        using var cts = new System.Threading.CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { } 
        };

        BotHandlers.BotClient = botClient;
        botClient.StartReceiving(
            updateHandler: BotHandlers.HandleUpdateAsync,
            pollingErrorHandler: BotHandlers.HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        cts.Cancel();
    }
}
