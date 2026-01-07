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

    public LogbookController(string configFileName = "userConfig.json")
    {
        ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + configFileName;
        if (!File.Exists(ConfigFileLocation))
        {
            Console.WriteLine("Cannot load userConfig.json, reverting to default config.");
            ConfigFileLocation = AppDomain.CurrentDomain.BaseDirectory + "defaultConfig.json";
        }

        var json = File.ReadAllText(path: ConfigFileLocation);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
        Boats = JsonSerializer.Deserialize<UserConfig>(json, options).Boats;
    }
}

public class UserConfig
{
    public Boat[] Boats { get; set; }
}