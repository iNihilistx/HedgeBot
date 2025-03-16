using Discord.WebSocket;
using Discord;

class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly Program _bot;
    // declaring a dictionary with string keys and Func<SocketMessage, Task> as values
    // Func<SocketMessage, Task>> is a delagate that represents a function. SocketMessage the param the function takes, representing an argment being SocketMessage and task indicates it'll be async
    private readonly Dictionary<string, Func<SocketMessage, Task>> _commands;

    public CommandHandler(DiscordSocketClient client, Program bot)
    {
        _client = client;
        _bot = bot;
        _client.MessageReceived += HandleCommandAsync;
        // initalise dictionary with case-insentive string comparison
        _commands = new Dictionary<string, Func<SocketMessage, Task>>(StringComparer.OrdinalIgnoreCase)
        {
            // test is the key, the value is a reference to the method matching the Func<SocketMessage, Task>
            { "$test", TestCommand},
            { "$help", HelpCommand },
            { "$ping", PingCommand }
        };
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        if (message.Author.IsBot || message.Author.IsWebhook) return;
        string prefix = "$";
        if (!message.Content.StartsWith(prefix)) return;

        string command = message.Content.Split(' ')[0]; // extract command
        if (_commands.TryGetValue(command, out var commandFunction))
        {
            await commandFunction(message);
        }
        else
        {
            await message.Channel.SendMessageAsync($"Invalid command, use {prefix}help to see available commands");
            await _bot.LogAsync($"Unknown command by: {message.Author.Username}");
        }
    }

    private async Task TestCommand(SocketMessage message)
    {
        await message.Channel.SendMessageAsync($"Fired in response to: {message.Author.Username}");
        await _bot.LogAsync($"Test command: {message}");
    }

    private async Task HelpCommand(SocketMessage message)
    {
        var embed = new EmbedBuilder()
        {
            Author = new EmbedAuthorBuilder()
            {
                Name = "Help Command",
            },
            Description = "Help Command",
            Color = Color.Teal
        };
        string commandList = ". **$test** - Run the test to check the bots working \n" +
                             ". **$help** - Shows this helpful help message\n" +
                             ". **$ping** - Run a ping test to see bot latency";
        embed.AddField("Commands", commandList);
        await message.Channel.SendMessageAsync("", false, embed.Build());
    }

    private async Task PingCommand(SocketMessage message)
    {
        var startTime = System.Diagnostics.Stopwatch.StartNew();

        var response = await message.Channel.SendMessageAsync("Executing ping test...");
        startTime.Stop();
        var pingTime = startTime.ElapsedMilliseconds;

        var pingResponse = $"**Ping Time:** {pingTime}ms";
        await response.ModifyAsync(msg => msg.Content = pingResponse);
    }
}