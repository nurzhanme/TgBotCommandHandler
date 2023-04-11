# TgBotCommandHandler

is a library that provides easy-to-use command handlers for Telegram bots commands from User

If you like this project please give a star and a cup of coffee =)

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/nurzhanme)

## Installation

[![NuGet Badge](https://buildstats.info/nuget/TgBotCommandHandler)](https://www.nuget.org/packages/TgBotCommandHandler/)

To install TgBotCommandHandler, you can use the NuGet package manager in Visual Studio. Simply search for "TgBotCommandHandler" and click "Install".

Alternatively, you can install TgBotCommandHandler using the command line:

```
Install-Package TgBotCommandHandler
```

## Getting Started

1. Register your command handler classes using `botClient.RegisterCommand<MyCommandHandler>` where `MyCommandHandler` is a subclass of `CommandHandler` and `botClient` is an instance of `ITelegramBotClient` 
2. initialize the `botClient.HandleCommands(update)` where `update` is an instance of `Telegram.Bot.Types.Update`

### Example

```c#
var botClient = new TelegramBotClient("YOUR_BOT_TOKEN");

botClient.RegisterCommand<MyCommandHandler>();
botClient.InitializeCommands(commandHandler);

botClient.StartReceiving(updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cancellationToken);
```

⚠️ NOTE: do NOT put your Token directly to your source code.

```c#
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;
    await botClient.HandleCommands(update);
}
```

#### Command Handlers
```csharp
public class MyCommandHandler : CommandHandler
{

    // All command methods MUST be async, have a return type of Task, have only a Command  as a parameter,
    // and have the [Command] attribute.
    // The parameter for the [Command] attribute indicates what invokes this method. DO NOT specify a prefix here.
    
    // When a user invokes the /hello command, the bot will respond with "Hello".
    [Command("hello")]
    public async Task HandleHelloCommand(CommandContext context)
    {
        Console.WriteLine("hello command request.");
        await context.RespondAsync("Hello");
    }

    // When a user invokes the /world command, the bot will respond with "World".
    [Command("world")]
    public async Task HandleWorldCommand(CommandContext context)
    {
        Console.WriteLine("world command request.");
        await context.RespondAsync("World");
    }

}
```