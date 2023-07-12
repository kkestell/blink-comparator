using System.Text.Json;
using File = System.IO.File;

namespace BlinkComparator;

public class Program
{
    public static async Task Main(string[] args)
    {
        var envPath = Environment.GetEnvironmentVariable("BLINK_COMPARATOR_CONFIG");
        var defaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config/blink-comparator/config.json");
        var configPath = string.IsNullOrEmpty(envPath) ? defaultConfigPath : envPath;

        if (!File.Exists(configPath))
        {
            Console.Error.WriteLine($"Config file not found at {configPath}");
            Console.Error.WriteLine(
                "Please create a config file at the above path or set the BLINK_COMPARATOR_CONFIG environment variable to the path of your config file.");
            Environment.Exit(1);
        }

        var config = JsonSerializer.Deserialize<Config>(await File.ReadAllTextAsync(configPath));

        if (config is null)
        {
            Console.Error.WriteLine("Invalid config file!");
            Environment.Exit(1);
        }

        var comparator = new Comparator(config);
        await comparator.Compare();
    }
}