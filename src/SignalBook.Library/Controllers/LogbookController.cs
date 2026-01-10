using System.Drawing;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;

namespace SignalBook.Controllers;

public class LogbookController
{
    public Boat[] Boats { get; set; }
    public string[] RadioLog { get; set; }
    public string ConfigFileLocation { get; set; }

    public void LoadConfig(string? configFileName = "userConfig.json")
    {
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

    public void EditConfig() 
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
}

public class UserConfig
{
    public Boat[] Boats { get; init; }
}
