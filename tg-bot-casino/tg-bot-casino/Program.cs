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
                                {
                                        new InlineKeyboardButton[] // тут создаем массив кнопок
                                        {
                                            InlineKeyboardButton.WithUrl("Рандомайзер", "https://www.random.org/"),
                                            InlineKeyboardButton.WithCallbackData("Ваше место", "button1"),
                                        },
                                        new InlineKeyboardButton[]
                                        {
                                            InlineKeyboardButton.WithCallbackData("Другие розыгрыши", "button2"),
                                            InlineKeyboardButton.WithCallbackData("Результаты", "button3"),
                                        },
                                });

        Console.WriteLine($"Бот запущен!");
        Console.ReadLine();
        Console.WriteLine("Бот остановлен");
    }


    private static async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                switch (update.Message.Text)
                {
                    case ("/start"):
                        {
                            await Console.Out.WriteLineAsync($"Пришло сообщение от пользователя: {update.Message.From.Username}");
                            await client.SendTextMessageAsync(update.Message.From.Id, "Здравствуйте");

                            break;
                        }
                    case ("/count"):
                        {
                            count++;
                            break;

                        }
                    case ("/ping"):
                        {
                            await Console.Out.WriteLineAsync($"Пришло сообщение от пользователя: {update.Message.From.Username}");

                            await client.SendTextMessageAsync(update.Message.From.Id, $"{count}");
                            break;

                        }
                    case ("/keyboard"):
                        {
                            var inlineKeyboard = keyboard;
                            await client.SendTextMessageAsync(update.Message.From.Id, $"Вот ваша клавиатура", replyMarkup: inlineKeyboard);
                            break;

                        }
                }

                return;

            case UpdateType.CallbackQuery:
                {
                    var callbackQuery = update.CallbackQuery;
                    var user = callbackQuery.From;
                    var chat = callbackQuery.Message.Chat;
                    switch (callbackQuery.Data)
                    {
                        case "button1":
                            {
                                await Console.Out.WriteLineAsync($"Пользователь нажал на кнопку btn1: {user.Username}");
                                await client.EditMessageTextAsync(chat.Id, callbackQuery.Message.MessageId, "<b>Вы</b>", replyMarkup: keyboard, parseMode: ParseMode.Html);


                                break;

                            }
                    }
                }
                return;
        }

    }

    private static Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
    {
        throw new NotImplementedException();
    }

}