using System.Reflection;
using SignalBook.Controllers;
using Spectre.Console;

namespace SignalBook.Library.Controllers;

/// <summary>
/// The terminal controller responsible for handling user input and coordinating between the TimeController and LogbookController.
/// </summary>
public class TerminalController
{
    /// <summary>
    /// The TimeController instance. See <see cref="TimeController"/>
    /// </summary>
    private readonly TimeController _timeController;
    /// <summary>
    /// The LogbookController instance. See <see cref="LogbookController"/>
    /// </summary>
    private readonly LogbookController _logbookController;

    /// <summary>
    /// The initialiser for the TerminalController. Prompts the user for initial time and config file.
    /// </summary>
    public TerminalController()
    {
        _timeController = new TimeController(PromptUser("[yellow]Enter the new time (HH:MM:SS). Leave blank to use current system time.[/]"));
        _logbookController = new LogbookController();
        _logbookController.LoadConfig(PromptUser("[yellow]Enter the name of the config file.[/]"));
        Start();
    }

    /// <summary>
    /// Starts the main command loop, prompting the user for input until they choose to exit.
    /// </summary>
    private void Start()
    {
        while (true)
        {
            string input = AnsiConsole.Ask<string>("[yellow]cmd>[/]");
            input = input.Trim().ToLower();
            if (!HandleUserInput(input))
            {
                break;
            }
        }

        Stop();
    }

    /// <summary>
    /// Stops the terminal controller and cleans up resources.
    /// </summary>
    private void Stop()
    {
        _timeController.StopTimer();
    }

    /// <summary>
    /// The main terminal input handler.
    /// Processes user commands and delegates to the appropriate controller methods.
    /// </summary>
    /// <param name="input">The user's input into the console.</param>
    /// <returns>Boolean for if the controller should continue checking for user input. Used for the exit process.</returns>
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
                _timeController.InitialiseTime(PromptUser("Enter the new time (HH:MM:SS). Leave blank to use current system time."));
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
                _logbookController.LoadConfig(PromptUser("Enter the name of the config file."));
                return true;

            case "config save":
                _logbookController.SaveConfig(PromptUser("Enter the name of the config file."));
                return true;

            case "log":
                return LogMode();

            case "log show":
                foreach (var entry in _logbookController.RadioLog)
                {
                    AnsiConsole.MarkupLine(entry);
                }

                return true;

            case "log load":
                _logbookController.LoadLog(PromptUser("Enter the name of the log file."));
                return true;

            case "log save":
                _logbookController.SaveLog();
                AnsiConsole.MarkupLine("[green dim]Logbook saved.[/]");
                return true;

            case "info":
                string version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
                AnsiConsole.MarkupLine($"[blue]SignalBook \n Version: {version} \n Author: Harvey Walker \n License: MIT[/]");
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
                                       info            - Show application information
                                       help            - Show this help message
                                       exit, quit      - Exit the application
                                       """);
                return true;
            case "exit":
            case "quit":
                if (_logbookController.SavedLogEntriesCount <= _logbookController.RadioLog.Count)
                {
                    if (AnsiConsole.Confirm("There are unsaved log entries. Do you want to save them first? (Y/N)\n>>>"))
                    {
                        AnsiConsole.MarkupLine("[green dim]Saving logbook...[/]");
                        _logbookController.SaveLog();
                    }
                }
                return false;

            default:
                AnsiConsole.MarkupLine("[red]Unknown command. Type 'help' for a list of commands.[/]");
                return true;
        }
    }

    /// <summary>
    /// Enters "log mode" where the user can input log entries until they type "exit".
    /// </summary>
    /// <returns>Boolean for if the controller should contiunue checking for log entries. Used to exit back to the main loop.</returns>
    private bool LogMode()
    {
        AnsiConsole.MarkupLine("Log mode is activated. Type \"exit\" to exit.");
        while (true)
        {
            string? logInput = AnsiConsole.Ask<string>("[yellow]log>[/]");
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

    /// <summary>
    /// Prompts the user for input with a given prompt message.
    /// </summary>
    /// <param name="prompt">The text to be displayed for the user.</param>
    /// <returns>The user's input as a string.</returns>
    private static string PromptUser(string prompt)
    {
        var textPrompt = new TextPrompt<string>(
                $"[yellow]{prompt}\n>>>[/]")
            .AllowEmpty();
        var promptReturn = AnsiConsole.Prompt(textPrompt);
        return promptReturn;
    }
}

