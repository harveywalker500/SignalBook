namespace SignalBook.Controllers;

public class TerminalController
{
    public TimeController TimeController;

    public TerminalController()
    {
        Console.WriteLine(" --- SIGNALBOOK ---");
        TimeController = new TimeController();
    }
}