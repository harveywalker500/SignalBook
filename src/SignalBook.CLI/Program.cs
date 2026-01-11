using SignalBook.Library.Controllers;
using Spectre.Console;

namespace SignalBook.CLI;

public static class Program
{
    private static TerminalController? _terminalController;

    public static void Main(string[] args)
    {
        var banner = new FigletText("SignalBook")
            .Color(Color.DodgerBlue1);
        AnsiConsole.Write(banner);
        _terminalController = new TerminalController();
        _terminalController.Start();

    }
}