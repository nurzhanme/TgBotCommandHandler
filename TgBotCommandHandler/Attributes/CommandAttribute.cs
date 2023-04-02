namespace TgBotCommandHandler.Attributes;

/// <summary>
/// Marks a Task as a Command
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{

    /// <summary>
    /// Determines what invokes the command to be called
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// New Command attribute
    /// </summary>
    /// <param name="value">Determines what invokes the command to be called, without prefix</param>
    public CommandAttribute(string value)
    {
        Value = value;
    }

}