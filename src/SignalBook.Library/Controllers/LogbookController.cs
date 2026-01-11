using System.Text.Json;
using Spectre.Console;

namespace SignalBook.Controllers;

public class LogbookController
{
    public static Boat[] Boats { get; set; }
    public List<string> RadioLog { get; set; }
    public string ConfigFileLocation { get; set; }

    public LogbookController()
    {
        Boats = Array.Empty<Boat>();
        RadioLog = new List<string>();
    }

    public void LoadConfig()
    {
        string? configFileName = PromptUser("Enter the configuration file name (or press Enter to use default)");

        if (!string.IsNullOrEmpty(configFileName))
        {
            ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
            if (!File.Exists(ConfigFileLocation))
            {
                Console.WriteLine($"Cannot load {configFileName}, reverting to default config.");
                ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + "defaultConfig.json";
            }

        }
        else
        {
            ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + "defaultConfig.json";
        }

        if (!File.Exists(ConfigFileLocation))
        {
            throw new FileNotFoundException("Default config file is missing. Aborting.");
        }

        var json = File.ReadAllText(path: ConfigFileLocation);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        Boats = JsonSerializer.Deserialize<UserConfig>(json, options).Boats;
    }

    public static void EditConfig()
    {
        for (int i = 0; i < Boats.Length; i++)
        {
            Console.WriteLine($"Enter boat number for boat {i + 1}:");
            var newBoatNumber = Console.ReadLine();
            if (!string.IsNullOrEmpty(newBoatNumber))
            {
                Boats[i].Number = newBoatNumber;
            }

            Console.WriteLine($"Enter radio operator for boat {i + 1}:");
            var newRadioOperator = Console.ReadLine();
            if (!string.IsNullOrEmpty(newRadioOperator))
            {
                Boats[i].RadioOperator = newRadioOperator;
            }

            Console.WriteLine($"Enter colour string for boat {i + 1}:");
            var newColourString = Console.ReadLine();
            if (!string.IsNullOrEmpty(newColourString))
            {
                Boats[i].ColourString = newColourString;
            }
        }
    }

    public void SaveConfig(string configFileName = "userConfig.json")
    {
        ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        var json = JsonSerializer.Serialize(
            new UserConfig { Boats = Boats }, options
        );
        File.WriteAllText(ConfigFileLocation, json);
    }

    public void Log(string message, TimeController timeController)
    {
        var timestamp = timeController.Time.ToString(format: "HH:mm:ss");
        RadioLog.Add($"{timestamp}: {message}");
    }

    public void LoadLog()
    {
        var logFileName = PromptUser("Enter the name of the log file.");
        var logFileLocation = AppDomain.CurrentDomain.BaseDirectory + logFileName;
        StreamReader logFile = new StreamReader(logFileLocation);
        while (logFile.EndOfStream == false)
        {
            RadioLog.Add(logFile.ReadLine());
        }
    }

    public void SaveLog(string logFileName = "Radio Log.txt")
    {
        var logFileLocation = AppDomain.CurrentDomain.BaseDirectory + logFileName;
        StreamWriter logFile = new StreamWriter(logFileLocation);
        foreach (string line in RadioLog)
        {
            logFile.WriteLine(line);
        }
        logFile.Close();
    }

    private string PromptUser(string prompt)
    {
        var textPrompt = new TextPrompt<string>(
                $"[yellow]{prompt}\n>>>[/]")
            .AllowEmpty();
        var promptReturn = AnsiConsole.Prompt(textPrompt);
        return promptReturn;
    }
}

public class UserConfig
{
    public Boat[] Boats { get; init; }
}
