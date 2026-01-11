namespace SignalBook.Controllers;

/// <summary>
/// The user configuration wrapper to pass information from the config file to the <see cref="LogbookController"/>.
/// </summary>
public class UserConfig
{
    public Boat[] Boats { get; init; }
}