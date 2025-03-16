using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.Diagnostics;


class Program
{
    private readonly DiscordSocketClient _client;
    private readonly string _token;
    private readonly CommandHandler _commandHandler;

    public Program(DiscordSocketClient client, string token)
    {
        _client = client;
        _token = token;
        _commandHandler = new CommandHandler(client, this); // pass this so commandHandler can use its to log

        // Handlers
        _client.Log += LogAsync;
    }

    static async Task Main(string[] args)
    {
        string token = BotHelper.RetreiveToken();
        var constructor = new BotHelper();
        var client = constructor.CreateClient();
        var bot = new Program(client, token);
        await bot.StartAsync();

        Console.WriteLine($"Bot is running, press any key to terminate session...");
        Console.ReadKey();

        await bot.StopASync();
    }

    private async Task StartAsync()
    {
        await _client.LoginAsync(TokenType.Bot, _token);
        await _client.StartAsync();
    }

    // log Discord.Net system logs
    public Task LogAsync(LogMessage message)
    {
        Console.WriteLine($"[{DateTime.Now}] {message}");
        return Task.CompletedTask;
    }

    // log commands/ usernmes/ etc
    public Task LogAsync(string logMessage)
    {
        Console.WriteLine($"[{DateTime.Now}] {logMessage}");
        return Task.CompletedTask;
    }

    private async Task StopASync()
    {
        await _client.StopAsync();
    }
}