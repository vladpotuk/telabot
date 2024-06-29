using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public static class BotHandlers
{
    public static ITelegramBotClient? BotClient;
    private static Dictionary<long, SeaBattleGame> games = new Dictionary<long, SeaBattleGame>();

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, System.Threading.CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message!.Text != null)
        {
            await Bot_OnMessage(update.Message);
        }
    }

    public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private static async Task Bot_OnMessage(Message message)
    {
        long chatId = message.Chat.Id;

        if (!games.ContainsKey(chatId))
        {
            games[chatId] = new SeaBattleGame();
            await BotClient!.SendTextMessageAsync(
                chatId: chatId,
                text: "Нова гра почалася! Розташовуйте свої кораблі."
            );

            await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: games[chatId].GetPlayerGrid()
            );
        }
        else
        {
            var game = games[chatId];
            string response = game.ProcessMove(message.Text ?? string.Empty);
            await BotClient!.SendTextMessageAsync(
                chatId: chatId,
                text: response
            );

            await BotClient.SendTextMessageAsync(
                chatId: chatId,
                text: game.GetEnemyGrid()
            );
        }
    }
}
