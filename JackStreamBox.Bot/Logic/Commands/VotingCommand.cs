using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using JackStreamBox.Util.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class VotingCommand : BaseCommandModule
    {
        private DiscordClient? _client;
        [Command("startvote")]
        public async Task StartVote(CommandContext context)
        {
            TimeSpan span = TimeSpan.FromSeconds(3);
            //End Game
            JackStreamBoxUtility.CloseGame();
            //Start Vote 60 sec
            _client = context.Client;


            PackGame[] games = PackInfo.GetRandomGames(5);
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = $"What game will we played next?\n{GameText(games)}"

            };



            var pollMessage  = await context.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);
            DiscordEmoji[] emojis = GetEmojis();
            foreach (var emoji in emojis)
            {
                await pollMessage.CreateReactionAsync(emoji).ConfigureAwait(false);
            }

            var interactivty = context.Client.GetInteractivity();
            var result = await interactivty.CollectReactionsAsync(pollMessage, span);
            var distinct = result.Distinct();

            Reaction[] results = result.Where(x => x.Total == result.Max(obj => obj.Total)).ToArray();


            var random = new Random();
            var pollWinner = results[random.Next(results.Length)];
            PackGame Winner = ReactionToId(games, pollWinner);

            await context.Channel.SendMessageAsync($"Winner is {Winner.Name}\n Starting now.");

            await JackStreamBoxUtility.OpenGame(Winner.Id);

            //Get Reactions
            //Pick Game
        }

        private PackGame ReactionToId(PackGame[] games, Reaction pollWinner)
        {
            int index = 0;
            switch (pollWinner.Emoji)
            {
                case ":one:": index = 0; break;
                case ":two:": index = 1; break;
                case ":three:": index = 2; break;
                case ":four:": index = 3; break;
                case ":five:": index = 4; break;
            }
            return games[index];
        }

        private DiscordEmoji[] GetEmojis()
        {
            if (_client == null) return Array.Empty<DiscordEmoji>(); 
            DiscordEmoji[] emojis =
            {
                DiscordEmoji.FromName(_client, ":one:"),
                DiscordEmoji.FromName(_client, ":two:"),
                DiscordEmoji.FromName(_client, ":three:"),
                DiscordEmoji.FromName(_client, ":four:"),
                DiscordEmoji.FromName(_client, ":five:")
            };

            return emojis;
        }

        private string GameText(PackGame[] games)
        {
            DiscordEmoji[] emojis = GetEmojis();
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < emojis.Length; i++)
            {
                sb.AppendLine($"{emojis[i]} {games[i].Name}");
            }

            return sb.ToString();
        }
    }
}
