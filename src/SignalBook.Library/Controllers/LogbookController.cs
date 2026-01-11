using System.Text.Json;
using Spectre.Console;

namespace SignalBook.Controllers;

/// <summary>
/// The controller responsible for managing the logbook.
/// </summary>
/// <remarks>
/// This class handles all aspects of the logging process, including loading the config, which contains the boat details
/// and radio operators, logging messages with timestamps <see cref="TimeController"/>, and saving/loading the log to/from a text file.
/// </remarks>
public class LogbookController
{
    /// <summary>
    /// The list of boats, their associated radio operators and colours.
    /// </summary>
    public static Boat[] Boats { get; set; }

    /// <summary>
    /// A list of text log entries.
    /// </summary>
    public List<string> RadioLog { get; set; }
    /// <summary>
    /// The location of the configuration file.
    /// </summary>
    public string ConfigFileLocation { get; set; }

    public int SavedLogEntriesCount { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogbookController"/> class.
    /// </summary>
    public LogbookController()
    {
        Boats = Array.Empty<Boat>();
        RadioLog = new List<string>();
    }

    /// <summary>
    /// Prompts the user to load the configuration file, and attempts to load it.
    /// </summary>
    /// <remarks>
    /// If the specified file cannot be found, it falls back to a default configuration file.
    /// Allows the user to not specify a file, in which case the default configuration file is used.
    /// </remarks>
    /// <exception cref="FileNotFoundException">Thrown if neither the provided configuration file or the default configuration file exists.</exception>
    public void LoadConfig(string configFileName)
    {
        if (string.IsNullOrEmpty(configFileName))
        {
            AnsiConsole.MarkupLine("[red] No config file specified, reverting to default config. [/]");
            configFileName = "defaultConfig.json";
            ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
        }
        else
        {
            ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
            if (!File.Exists(ConfigFileLocation))
            {
                AnsiConsole.MarkupLine($"[red]Cannot load {configFileName}, reverting to default config.[/]");
                configFileName = "defaultConfig.json";
                ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
            }
        }

        if (!File.Exists(ConfigFileLocation))
        {
            AnsiConsole.WriteException(new FileNotFoundException("Default config file is missing. Aborting."));
        }

        var json = File.ReadAllText(path: ConfigFileLocation);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        Boats = JsonSerializer.Deserialize<UserConfig>(json, options).Boats;
        AnsiConsole.MarkupLine($"[green] Successfully loaded {configFileName}. [/]");
    }

    /// <summary>
    /// Prompts the user to edit the boat configuration interactively.
    /// </summary>
    /// <remarks>
    /// This uses it's own instance of <see cref="PromptUser"> which should be changed.
    /// </remarks>
    public static void EditConfig()
    {
        for (int i = 0; i < Boats.Length; i++)
        {
            var newBoatNumber = PromptUser($"Enter boat number for boat {i + 1}");
            if (!string.IsNullOrEmpty(newBoatNumber))
            {
                Boats[i].Number = newBoatNumber;
            }
            else
            {
                AnsiConsole.MarkupLine("[red] Warning: Boat number was empty! [/]");
            }

            var newRadioOperator = PromptUser($"Enter radio operator for boat {i + 1}");
            if (!string.IsNullOrEmpty(newRadioOperator))
            {
                Boats[i].RadioOperator = newRadioOperator;
            } else {
                AnsiConsole.MarkupLine("[red] Warning: Radio operator was empty! [/]");
            }

            var newColourString = PromptUser($"Enter colour string for boat {i + 1}");
            if (!string.IsNullOrEmpty(newColourString))
            {
                Boats[i].ColourString = newColourString;
            } else {
                AnsiConsole.MarkupLine("[red] Warning: Colour string was empty! [/]");
            }
        }
    }

    /// <summary>
    /// Saves the current boat configuration to a JSON file.
    /// </summary>
    /// <param name="configFileName">The name of the config file for the user. </param>
    public void SaveConfig(string configFileName = "userConfig.json")
    {
        ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        var json = JsonSerializer.Serialize(
            new UserConfig { Boats = Boats }, options
        );
        File.WriteAllText(ConfigFileLocation, json);
        AnsiConsole.MarkupLine("[green dim]Configuration saved.[/]");
    }

    /// <summary>
    /// Logs a message in the radio log with a timestamp from the provided <see cref="TimeController"/>.
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="timeController">The <see cref="TimeController"/> class to write the config message with.</param>
    public void Log(string message, TimeController timeController)
    {
        var timestamp = timeController.Time.ToString(format: "HH:mm:ss");
        RadioLog.Add($"{timestamp}: {message}");
    }

    /// <summary>
    /// Loads a log file specified by the user into the radio log.
    /// </summary>
    /// <param name="logFileName">File name of the radio log.</param>
    public void LoadLog(string logFileName)
    {
        var logFileLocation = AppDomain.CurrentDomain.BaseDirectory + logFileName;
        StreamReader logFile = new StreamReader(logFileLocation);
        while (logFile.EndOfStream == false)
        {
            RadioLog.Add(logFile.ReadLine());
        }
    }

    /// <summary>
    /// Saves the current radio log to a text file.
    /// </summary>
    /// <param name="logFileName">File name of the radio log.</param>
    public void SaveLog(string logFileName = "Radio Log.txt")
    {
        var logFileLocation = AppDomain.CurrentDomain.BaseDirectory + logFileName;
        StreamWriter logFile = new StreamWriter(logFileLocation);
        foreach (string line in RadioLog)
        {
            logFile.WriteLine(line);
        }
        logFile.Close();
        SavedLogEntriesCount = RadioLog.Count;
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