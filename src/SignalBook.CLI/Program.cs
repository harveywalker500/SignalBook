using SignalBook.Library.Controllers;
using Spectre.Console;

namespace SignalBook.CLI;
/// <summary>
/// Main entry point for the SignalBook CLI application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The terminal controller instance. See <see cref="TerminalController"/>
    /// </summary>
    private static TerminalController? _terminalController;

    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    /// <param name="args">Any arguments passed into the commandline.</param>
    public static void Main(string[] args)
    {
        var banner = new FigletText("SignalBook")
            .Color(Color.DodgerBlue1);
        AnsiConsole.Write(banner);
        _terminalController = new TerminalController();
    }
}