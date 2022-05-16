using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Requests;

using ScheduleBot;

using System.Text.RegularExpressions;

var botClient = new TelegramBotClient("5379652671:AAHoTF7WICMDmy9_7MPVYA9JB-aOuzabkrA");

var receivingOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

var cancelation = new CancellationTokenSource();

// start receiving messages
botClient.StartReceiving(
    updateHandler: UpdateHandler,
    errorHandler: ErrorHandler,
    receiverOptions: receivingOptions,
    cancellationToken: cancelation.Token);

var bot = await botClient.GetMeAsync();

Console.WriteLine($"{bot.FirstName} is started");

// run it untill we hit any button
Console.ReadLine();

cancelation.Cancel();


async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // process only messages
    if (update.Type != UpdateType.Message)
        return;

    var chatId = update.Message.Chat.Id;
    var username = update.Message.Chat.Username;
    var textMessage = update.Message.Text;
    

    if (update.Message.Type == MessageType.Text)
    {
        // send a special command
        if (textMessage == "/schedule")
        {
            // open keyboard markup
            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
            {
                new KeyboardButton[]{Week.Monday, Week.Tuesday},
                new KeyboardButton[]{Week.Wednesday, Week.Thursday, Week.Friday}
            });
            Message message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Выберите расписание на день недели",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken); ;
        }
        else if (Week.IsValid(textMessage))
        {
            Message message = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: Week.GetSchedule(textMessage),
                cancellationToken: cancellationToken);
        }
    }

}

Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var message = exception switch
    {
        ApiRequestException apiRequestException => $"Api Error: {apiRequestException.ErrorCode}\n {apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(message);

    return Task.CompletedTask;
}