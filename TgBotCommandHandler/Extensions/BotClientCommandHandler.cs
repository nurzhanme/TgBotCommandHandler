using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgBotCommandHandler.Attributes;
using TgBotCommandHandler.Types;
using TgBotCommandHandler.Utils;

namespace TgBotCommandHandler.Extensions;

public static class BotClientCommandHandler
{
    private static readonly Dictionary<string, (Type type, MethodInfo methodInfo)> handlersDictionary = new();

    public static void RegisterCommand<T>(this ITelegramBotClient client) where T : CommandHandler
    {
        var methods = typeof(T).GetMethods().Where(method =>
                method.GetParameters()
                    .Length == 1
                && CommandHandlerUtils.IsSameOrSubclass(method.GetParameters()[0].ParameterType,
                    typeof(CommandContext))
                && method.IsAsync()
                && CommandHandlerUtils.IsSameOrSubclass(method.ReturnType, typeof(Task))
                && method.GetCustomAttributes(typeof(CommandAttribute), false)?.Any() == true)
            .ToList();

        foreach (var method in methods)
        {
            var commandAttribute = method
                    .GetCustomAttribute(typeof(CommandAttribute), false)
                as CommandAttribute;

            var key = commandAttribute?.Value;

            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            if (handlersDictionary.ContainsKey(key))
            {
                continue;
            }

            handlersDictionary.Add(key, (typeof(T), method));
        }
    }

        
    public static async Task HandleCommands(this ITelegramBotClient client, Update e)
    {
        var message = e.Message;
            
        if (message?.Text?[0] != '/')
        {
            return;
        }

        var command = message.Text.Substring(1).ToLower();

        if (!handlersDictionary.ContainsKey(command))
        {
            return;
        }

        var val = handlersDictionary[command];
        var commandHandler = (CommandHandler)Activator.CreateInstance(val.type);


        var helper = new CommandContext(message, client);

        await (Task)val.methodInfo.Invoke(commandHandler, new object[] { helper });

    }

}