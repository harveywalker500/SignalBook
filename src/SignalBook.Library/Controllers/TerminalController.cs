using SignalBook.Controllers;

namespace SignalBook.Library.Controllers;

public class TerminalController
{
    public TimeController TimeController;
    public LogbookController LogbookController;

    public TerminalController()
    {
        Console.WriteLine(" --- SIGNALBOOK ---");
        TimeController = new TimeController();

        LogbookController = new LogbookController();
        Console.WriteLine("Enter the configuration file name (or press Enter to use default):");
        string? configFileName = Console.ReadLine();
        LogbookController.LoadConfig(configFileName);
    }
}