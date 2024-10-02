using System.Security.AccessControl;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

class Programm
{
    private static ITelegramBotClient? client;
    private static ReceiverOptions? receiverOptions;
    private static string token = "7444413593:AAFHLlvqgpqVcupxGKkZOzbyhLVVgM1vyGA";
    private static InlineKeyboardMarkup keyboard;
    private static int count = 0;
    public static void Main()
    {
        client = new TelegramBotClient(token);
        receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new[]
                        {
                UpdateType.Message,
                UpdateType.CallbackQuery
            }
        };
        using var cts = new CancellationTokenSource();
        client.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cts.Token);

        keyboard = new InlineKeyboardMarkup(
                                new List<InlineKeyboardButton[]>()
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Участвовать", $"participate_{drawName}"),
                InlineKeyboardButton.WithCallbackData("Отписаться", $"withdraw_{drawName}")
            }
        });

        Console.WriteLine($"Бот запущен!");
        Console.ReadLine();
        Console.WriteLine("Бот остановлен");
    }


private static async Task CheckWinner(ITelegramBotClient client, Draw draw)
    {
        if (draw.Participants.Any())
        {
            Random rand = new Random();
            int winnerIndex = rand.Next(draw.Participants.Count);
            string winnerName = draw.Participants[winnerIndex];
            long winnerId = draw.ParticipantIds[winnerIndex];

            string messageText = $"Поздравляем! Вы победитель розыгрыша '{draw.Name}'!";
            await client.SendTextMessageAsync(winnerId, messageText);
            foreach (var participantId in draw.ParticipantIds)
            {
                if (participantId != winnerId)
                {
                    await client.SendTextMessageAsync(participantId, $"Вы не выиграли в розыгрыше '{draw.Name}'. Спасибо за участие!");
                }
            }

            string participantList = string.Join(", ", draw.Participants);

            foreach (var participantId in draw.ParticipantIds)
            {
                await client.SendTextMessageAsync(participantId, $"Результаты розыгрыша '{draw.Name}\nПобедитель - {winnerName}.\n\n\nПолный список участников: {participantList}");
            }
        }
        else
        {
            string noParticipantsMessage = $"В розыгрыше '{draw.Name}' нет участников.";
            foreach (var participantId in draw.ParticipantIds)
            {
                await client.SendTextMessageAsync(participantId, noParticipantsMessage);
            }
        }
    }

    private static Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }

}