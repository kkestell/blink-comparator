using System.Text.Json;
using AngleSharp;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace BlinkComparator;

public class Comparator
{
    private readonly Config _config;

    public Comparator(Config config)
    {
        _config = config;
    }

    public async Task Compare()
    {
        var state = LoadState();
        var newContent = await Fetch(_config.Url, _config.HtmlSelector);

        if (string.IsNullOrEmpty(state.Content))
        {
            state.Content = newContent;
            SaveState(state);
            Environment.Exit(0);
        }

        if (!newContent.Equals(state.Content))
        {
            await Notify($"Changes detected! Check it: {_config.Url}", _config.ApiKey, _config.ChatId);
            state.Content = newContent;
            state.Count = 0;
        }
        else
        {
            state.Count++;
            if (state.Count >= 96)
            {
                await Notify("Still no changes...", _config.ApiKey, _config.ChatId);
                state.Count = 0;
            }
        }

        SaveState(state);
    }
    
    private static string StateFilePath()
    {
        var envPath = Environment.GetEnvironmentVariable("BLINK_COMPARATOR_STATE");
        var defaultStatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".local/state/blink-comparator/state.json");
        return string.IsNullOrEmpty(envPath) ? defaultStatePath : envPath;
    }

    private static State LoadState()
    {
        var statePath = StateFilePath();

        if (!File.Exists(statePath))
            return new State();

        var loadedState = JsonSerializer.Deserialize<State>(File.ReadAllText(statePath));

        if (loadedState is null)
        {
            Console.Error.WriteLine("Invalid state file!");
            return new State();
        }

        return loadedState;
    }

    private static void SaveState(State state)
    {
        var statePath = StateFilePath();

        var stateDirectory = Path.GetDirectoryName(statePath);
        
        if (stateDirectory is not null)
            Directory.CreateDirectory(stateDirectory);
        
        var data = JsonSerializer.Serialize(state);
        File.WriteAllText(statePath, data);
    }

    private static async Task Notify(string message, string apiKey, long chatId)
    {
        var bot = new TelegramBotClient(apiKey);
        await bot.SendTextMessageAsync(new ChatId(chatId), message);
    }

    private static async Task<string> Fetch(string url, string htmlSelector)
    {
        var httpClient = new HttpClient();
        var content = await httpClient.GetStringAsync(url);

        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(content));

        var text = string.Empty;

        var elements = document.QuerySelectorAll(htmlSelector);
        foreach (var element in elements) text = element.TextContent.Trim();

        return text;
    }
}