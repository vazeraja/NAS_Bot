using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using NAS_Bot.Commands;
using Newtonsoft.Json;

namespace NAS_Bot
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string prefix { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);

                token = data.token;
                prefix = data.prefix;
            }
        }
    }

    internal sealed class JSONStructure
    {
        public string token { get; set; }
        public string prefix { get; set; }
    }

    internal class Program
    {
        private static DiscordClient client { get; set; }
        private static CommandsNextExtension commands { get; set; }

        private static async Task Main(string[] args)
        {
            await Bootstrap();
        }

        private static async Task Bootstrap()
        {
            JSONReader reader = new JSONReader();
            await reader.ReadJSON();

            Console.WriteLine(reader.token);

            client = new DiscordClient(new DiscordConfiguration
            {
                Token = reader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All,
            });
            client.Ready += ClientOnReady;

            commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] {reader.prefix},
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            });
            commands.RegisterCommands<TestCommands>();

            AttachShutdownHandlers();

            await client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static Task ClientOnReady(DiscordClient sender, ReadyEventArgs args) => Task.CompletedTask;

        private static void AttachShutdownHandlers()
        {
            // Capture Ctrl+C or closing the console
            Console.CancelKeyPress += async (sender, eventArgs) =>
            {
                eventArgs.Cancel = true; // Prevent immediate shutdown
                await GracefulShutdown();
            };

            // Capture process exit (e.g., closing the terminal window)
            AppDomain.CurrentDomain.ProcessExit += async (sender, eventArgs) => { await GracefulShutdown(); };
        }

        private static async Task GracefulShutdown()
        {
            if (client != null)
            {
                Console.WriteLine("Shutting down bot...");

                await client.DisconnectAsync(); // Disconnect from Discord

                Console.WriteLine("Bot disconnected successfully.");
            }
        }
    }
}