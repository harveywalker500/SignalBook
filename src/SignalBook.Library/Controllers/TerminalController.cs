using SignalBook.Controllers;
using Spectre.Console;

namespace SignalBook.Library.Controllers;

public class TerminalController
{
    private TimeController _timeController;
    private LogbookController _logbookController;

    public TerminalController()
    {
        _timeController = new TimeController();
        _logbookController = new LogbookController();
        _logbookController.LoadConfig();
    }

    public void Start()
    {
        while (true)
        {
            string? input = AnsiConsole.Ask<string>("[yellow]cmd>[/]");
            input = input.Trim().ToLower();
            if (!HandleUserInput(input))
            {
                break;
            }
        }

        Stop();
    }

    private void Stop()
    {
        _timeController.StopTimer();
        AnsiConsole.MarkupLine("[green dim]Saving logbook...[/]");
        _logbookController.SaveLog();
    }

    private bool HandleUserInput(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            AnsiConsole.MarkupLine("[red]Invalid input, cannot be null.[/]");
            return true;
        }
        input = input.Trim().ToLower();
        switch (input)
        {
            case "time":
                AnsiConsole.MarkupLine($"[yellow]Current time: {_timeController.Time:hh:mm:ss}[/]");
                return true;

            case "time set":
                _timeController.InitialiseTime();
                return true;

            case "config show":
                foreach (var boat in LogbookController.Boats)
                {
                    AnsiConsole.MarkupLine(boat.ToString());
                }
                return true;

            case "config edit":
                LogbookController.EditConfig();
                return true;

            case "config load":
                _logbookController.LoadConfig();
                return true;

            case "config save":
                _logbookController.SaveConfig();
                return true;

            case "exit":
            case "quit":
                return false;

            case "log":
                return LogMode();

            case "log show":
                foreach (var entry in _logbookController.RadioLog)
                {
                    AnsiConsole.MarkupLine(entry);
                }

                return true;

            case "log load":
                _logbookController.LoadLog();
                return true;

            case "log save":
                _logbookController.SaveLog();
                AnsiConsole.MarkupLine("[green dim]Logbook saved.[/]");
                return true;

            case "help":
                AnsiConsole.MarkupLine("""
                                       ----- Configuration commands -----
                                       config edit     - Edit boat configuration
                                       config show     - Show current boat configuration
                                       config load     - Load boat configuration from file
                                       config save     - Save boat configuration to file
                                       
                                       ----- Time commands -----
                                       time            - Show current time
                                       time set        - Set current time
                                       
                                       ----- Logbook commands -----
                                       log             - Enters log mode to input radio messages
                                       log show        - Show the current logbook entries
                                       log load        - Load the logbook from file
                                       log save        - Save the current logbook to file
                                       
                                       ------ Other commands -----
                                       help            - Show this help message
                                       exit, quit      - Exit the application
                                       """);
                return true;

            default:
                AnsiConsole.MarkupLine("[red]Unknown command. Type 'help' for a list of commands.[/]");
                return true;
        }
    }

    private bool LogMode()
    {
        AnsiConsole.MarkupLine("Log mode is activated. Type \"exit\" to exit.");
        while (true)
        {
            string? logInput = AnsiConsole.Ask<string>("[yellow]log>[/]");
            logInput = logInput.Trim().ToLower();
            if (logInput == "exit")
            {
                AnsiConsole.MarkupLine("Exiting log mode.");
                return true;
            }

            else if (string.IsNullOrWhiteSpace(logInput))
            {
                continue;
            }
            _logbookController.Log(logInput, _timeController);
        }
    }
}