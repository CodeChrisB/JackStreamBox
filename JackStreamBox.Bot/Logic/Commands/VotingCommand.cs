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

        private const int TIME = 20;
        #region Vote Declaration
        private const int REQUIRED_VOTES = 3;
        private int? CURRENT_PACK = null;
        private PackGame[]? games = null;
        private List<string> CURRENT_VOTES = new();
        public void AddVote(string str)
        {
            if (CURRENT_VOTES.Contains(str)) return;
            CURRENT_VOTES.Add(str);
        }
        public void ResetVote()
        {
            CURRENT_VOTES = new List<string>();
            CURRENT_PACK = null;
            games = null;
        }


        private async Task LogVoteText(CommandContext context) { await context.Channel.SendMessageAsync($"Start Vote {CURRENT_VOTES.Count()}/{REQUIRED_VOTES}"); }
        #endregion

        private DiscordClient? _client;
        [Command("startvote")]
        [Description("Starts a new voting. !Closes the current game if there is one!")]
        [Requires(PermissionRole.ANYONE)]
        public async Task StartVote(CommandContext context, int pack)
        {
            await StartVoteWithPack(context, pack);
        }

        [Command("startvote")]
        public async Task StartVote(CommandContext context)
        {
            await StartVoteWithPack(context, -1);
        }
        private async Task StartVoteWithPack(CommandContext context, int pack)
        {
            int level = CommandLevel.RoleToLevel(context.Member.Roles);

            games = pack == -1 ? PackInfo.GetRandomGames(5) : PackInfo.GetPackInfo(pack).games;


            if (level >= (int)PermissionRole.HIGHLYTRUSTED)
            {
                //Tophost and higher
                await StartVoteNow(context, games);
                ResetVote();

            }
            else if (level >= (int)PermissionRole.TRUSTED)
            {
                //Level 3 and higher
                if (CURRENT_VOTES.Count == 0)
                {
                    AddVote(context.Member.Id.ToString());
                    await LogVoteText(context);
                    string messageStart = pack == -1 ? "Voting for Random games." :  $"Voting for **The Jackbox Party Pack {pack}**";
                    await context.Channel.SendMessageAsync($"{messageStart}\nUse **!vote** ");
                }
            }
            else
            {
                //Low trusted users
                await context.Channel.SendMessageAsync("Reach 'Level 3' Role to gain acess to this command\n(Gain Level 3 by being active in voice or text chat usually takes a week for an active user.)");
            }

        }
        private async Task StartVoteNow(CommandContext context, PackGame[] games)
        {
            TimeSpan span = TimeSpan.FromSeconds(TIME);
            //End Game
            JackStreamBoxUtility.CloseGame();
            //Start Vote 60 sec
            _client = context.Client;


            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = $"*What game will be played next?*\n(You have {TIME} seconds.---)\n\n{GameText(games)}"
            };



            var pollMessage = await context.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);





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
            var pollWinner = results.Length == 1 ? results[0] : results[random.Next(results.Length)];

            PackGame Winner = ReactionToId(games, pollWinner);

            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            await Logger($"Winner is {Winner.Name}\n");

            await JackStreamBoxUtility.OpenGame(Winner.Id, Logger);
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
                sb.AppendLine($"{emojis[i]} {games[i].Name}\n");
            }

            return sb.ToString();
        }
    }
}
