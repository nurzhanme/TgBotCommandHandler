using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TgBotCommandHandler.Types;

public class CommandContext
{
    /// <summary>
    /// Message that invoked the command
    /// </summary>
    public Message Message { get; private set; }
    /// <summary>
    /// Bot client
    /// </summary>
    public ITelegramBotClient BotClient { get; private set; }
    /// <summary>
    /// Id of the message that invoked the command
    /// </summary>
    public int MessageId => Message.MessageId;

    /// <summary>
    /// Chat of the message that invoked the command
    /// </summary>
    public Chat Chat => Message.Chat;
       
    /// <summary>
    /// Id of the chat of the message that invoked the command
    /// </summary>
    public long ChatId => Message.Chat.Id;

    /// <summary>
    /// Create a new command context
    /// </summary>
    /// <param name="message">Message that invoked the command</param>
    /// <param name="botClient">Bot client</param>
    public CommandContext(Message message, ITelegramBotClient botClient)
    {
        Message = message;
        BotClient = botClient;
    }

    /// <summary>
    /// Quick way to respond to the message
    /// </summary>
    /// <param name="text">Message Text</param>
    /// <param name="sendTyping">Show typing in chat</param>
    /// <param name="asReply">Send message as a reply to original message</param>
    /// <returns></returns>
    public async Task RespondAsync(string text, bool sendTyping = false, bool asReply = false)
    {
        int? messageId = null;

        if (sendTyping)
        {
            await BotClient.SendChatActionAsync(ChatId, ChatAction.Typing);
        }

        if (asReply)
        {
            messageId = MessageId;
        }

        await BotClient.SendTextMessageAsync(ChatId, text, replyToMessageId: messageId);
    }

}