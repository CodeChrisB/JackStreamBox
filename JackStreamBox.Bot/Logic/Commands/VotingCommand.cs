using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using JackStreamBox.Util.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class VotingCommand : BaseCommandModule
    {
        private DiscordClient? _client;
        [Command("startvote")]
        [Description("Starts a new voting. !Closes the current game if there is one!  Execution requires level 4.")]
        [Requires(PermissionRole.HIGHLYTRUSTED)]
        public async Task StartVote(CommandContext context)
        {

            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.HIGHLYTRUSTED)) return;

            TimeSpan span = TimeSpan.FromSeconds(3);
            //End Game
            JackStreamBoxUtility.CloseGame();
            //Start Vote 60 sec
            _client = context.Client;


            PackGame[] games = PackInfo.GetRandomGames(5);
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = $"What game will be played next?\n{GameText(games)}"
            };

            

            var pollMessage  = await context.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

          



            async Task Logger(string message)
            {
                //Set New Poll Data
                pollEmbed.Description = $"{pollEmbed.Description}\n{message}";
                //Delete Poll
                await pollMessage.ModifyAsync(null, pollEmbed.Build());       
                return;
            }



            DiscordEmoji[] emojis = GetEmojis();
            foreach (var emoji in emojis)
            {
                await pollMessage.CreateReactionAsync(emoji).ConfigureAwait(false);
            }
            //Get Reactions
            var interactivty = context.Client.GetInteractivity();
            var result = await interactivty.CollectReactionsAsync(pollMessage, span);
            var distinct = result.Distinct();

            Reaction[] results = result.Where(x => x.Total == result.Max(obj => obj.Total)).ToArray();

            //Pick Game               
            var random = new Random();
            var pollWinner =  results.Length==1 ? results[0] : results[random.Next(results.Length)];

            PackGame Winner = ReactionToId(games, pollWinner);

            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            await Logger($"Winner is {Winner.Name}\n");

            await JackStreamBoxUtility.OpenGame(Winner.Id,Logger );

            
            
        }

        private PackGame ReactionToId(PackGame[] games, Reaction pollWinner)
        {
            int index = 0;
            DiscordEmoji[] emojis = GetEmojis();

            for(int i = 0; i < emojis.Length;i++)
            {
                if (emojis[i].Name == pollWinner.Emoji.Name) index = i;
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
