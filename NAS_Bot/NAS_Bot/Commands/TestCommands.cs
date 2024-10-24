using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace NAS_Bot.Commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task MyFirstCommand(CommandContext context)
        {
            await context.Channel.SendMessageAsync("69");
        }

        [Command("msg")]
        public async Task EmbeddedMessage(CommandContext context)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = "Embedded Discord Message",
                Description = $"This command was executed by {context.User.Username}",
                Color = DiscordColor.Blue
            };

            await context.Channel.SendMessageAsync(embed: message);
        }

        [Command("cardgame")]
        public async Task CardGame(CommandContext context)
        {
            var userCard = new CardSystem();
            var userCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"Your card is {userCard.SelectedCard}",
                Color = DiscordColor.Lilac
            };

            await context.Channel.SendMessageAsync(embed: userCardEmbed);

            var botCard = new CardSystem();
            var botCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"The bot drew a {botCard.SelectedCard}",
                Color = DiscordColor.Blurple
            };

            await context.Channel.SendMessageAsync(embed: botCardEmbed);

            DiscordEmbedBuilder endMessage;
            if (userCard.SelectedNumber > botCard.SelectedNumber)
            {
                endMessage = new DiscordEmbedBuilder
                {
                    Title = "Congratulations, you won!",
                    Color = DiscordColor.Green,
                };
            }
            else
            {
                endMessage = new DiscordEmbedBuilder
                {
                    Title = "You lost",
                    Color = DiscordColor.Red,
                };
            }

            await context.Channel.SendMessageAsync(embed: endMessage);
        }
    }
}