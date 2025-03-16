using Discord.WebSocket;
using Discord;
using System.Text.Json;

class BotHelper
{
    public DiscordSocketClient CreateClient()
    {
        var config = GetConfig();
        return new DiscordSocketClient(config);
    }

    public static string RetreiveToken()
    {
        string fileName = "D:\\c#\\projects\\DiscordBot\\DiscordBot\\DiscordBot\\token.json";
        string jsonString = File.ReadAllText(fileName);
        var tokenData = JsonSerializer.Deserialize<TokenData>(jsonString);
        return tokenData?.Token;
    }

    private static DiscordSocketConfig GetConfig()
    {
        return new DiscordSocketConfig()
        {
            GatewayIntents = GatewayIntents.All,
            LogLevel = LogSeverity.Debug
        };
    }
}
