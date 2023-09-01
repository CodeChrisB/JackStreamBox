using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands._Helper.ChartBuilder;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using JackStreamBox.Util.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class VotingCommand : BaseCommandModule
    {
        #region Vote Declaration

        //Vote Props
        private List<string> voteCategories = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", /*"draw", "trivia", "talk", "fun"*/ };
        Dictionary<string, string> VotesOfPlayers = new Dictionary<string, string>();
        private PackGame[]? games = null;
        private int timeTillVoteEnd =0;

        //Discord Props
        private DiscordMessage PrePollMessage;
        private DiscordEmbedBuilder PrePollMessageData = new DiscordEmbedBuilder { };

   
        public void ResetVote()
        {
            VotesOfPlayers = new Dictionary<string, string>();
            games = null;
            PrePollMessage = null;
            ResetGameStartSteps();
        }
        #endregion

        #region Step
        public struct Step {
            public string Text;
            public bool Completed;
        }

        Step[] GameStartSteps = new Step[1];
        private string Winner = "";

        private void ResetGameStartSteps()
        {
            GameStartSteps = new Step[]
            {
                new Step { Text = BotMessage.StartingGamePack, Completed = false },
                new Step { Text = BotMessage.OpenedGamePack, Completed = false },
                new Step { Text = BotMessage.StartingGame, Completed = false },
                new Step { Text = BotMessage.GameOpend, Completed = false },
                new Step { Text = BotMessage.StartingStream, Completed = false },
                new Step { Text = BotMessage.AllFinished, Completed = false },
            };
        }
        #endregion





        [Command("vote")]
        [CoammandDescription($"Vote for the pack/category you want to play, when 4 players vote one of the voted categories will be picked. ",":ballot_box:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task Vote(CommandContext context, string voteCategory)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await context.Message.DeleteAsync();


            voteCategory = voteCategory.ToLower();

            if (voteCategories.IndexOf(voteCategory) > -1)
            {
                VotesOfPlayers[context.Member.Id.ToString()] = voteCategory;
            }
            else
            {
                await context.Channel.SendMessageAsync("use **!vote** for information what you can vote for.");
                
            }

            if (VotesOfPlayers.Values.ToList().Count == 1 && PrePollMessage == null)
            {
                ResetGameStartSteps();
                Task.Run(() => VoteOrCancel(context));
                PrePollMessageData = new DiscordEmbedBuilder
                {
                    Title = "Game Vote",
                    Description = $"Setting up the Poll",
                    Color = DiscordColor.Green,

                };
                PrePollMessage = await context.Channel.SendMessageAsync(embed: PrePollMessageData).ConfigureAwait(false);
                
            }
            UpdatePreMessage();
        }

        private void UpdatePreMessage()
        {
            if (PrePollMessage == null) return;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Time Left: {timeTillVoteEnd}s");

            int requiredVotes = BotData.ReadData(BotVals.REQUIRED_VOTES, 4);

            if(VotesOfPlayers.Values.Count<requiredVotes)
                sb.AppendLine($"Required Votes: {VotesOfPlayers.Values.Count}/{requiredVotes}");


            foreach ( var key in voteCategories )
                sb.AppendLine($"**!vote {key}** : {VotesOfPlayers.Count(x => x.Value == key)}");

            if (timeTillVoteEnd > 0)
            {
                PrePollMessageData.Description = sb.ToString();
                PrePollMessage.ModifyAsync(PrePollMessageData.Build());
            }
            else
            {
                if (PrePollMessage == null) return;
                PrePollMessage.DeleteAsync();
            }

        }

        public async Task VoteOrCancel(CommandContext context)
        {
            
            timeTillVoteEnd = BotData.ReadData(BotVals.VOTE_TIMER, 30);

            while (timeTillVoteEnd >= 0)
            {
                await Task.Delay(1000);
                UpdatePreMessage();
                timeTillVoteEnd--;
            }


            List<string> votes = VotesOfPlayers.Values.ToList();
             
            if(votes.Count >= BotData.ReadData(BotVals.REQUIRED_VOTES, 3))
            {
                string vote = votes[new Random().Next(votes.Count)];
                games = PackInfo.GetVotePack(vote);
                await VoteNow(context,games);
            } 
            else
            {

                await PlainEmbed
                    .CreateEmbed(context)
                    .Title("Not Enough Votes !")
                    .DescriptionAddLine($"Yikes... we need some more votes to start a game\n")
                    .DescriptionAddLine($"Next time try it with atleast {BotData.ReadData(BotVals.REQUIRED_VOTES, 3)} votes")
                    .DescriptionAddLine("Just trying to start a new vote to end the current game will revoke your bot rights.")
                    .BuildNDestroy(DestroyTime.REALLYSLOW);
            }
            ResetVote();
        }

        private async Task VoteNow(CommandContext context, PackGame[] games)
        {
            // End Game
            JackStreamBoxUtility.CloseGame();


            TimeSpan voteTimer = TimeSpan.FromSeconds(BotData.ReadData(BotVals.VOTE_TIMER, 30));
            TimeSpan pickTimer = TimeSpan.FromSeconds(BotData.ReadData(BotVals.PICK_TIMER, 30));

            DiscordEmoji[] emojis = GetEmojis(context);


            // Create embed
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = "Setting up the Poll"
            };
            var pollMessage = await context.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            // Add Reactions
            foreach (var emoji in emojis)
                await pollMessage.CreateReactionAsync(emoji);


            // Logger Setup Phase
            async Task Logger(VoteStatus status)
            {
                GameStartSteps[(int)status].Completed = true;

                var description = new StringBuilder($"Winner is {Winner}\n");

                foreach (var step in GameStartSteps)
                {
                    description.AppendLine($"{StatusEmoji(step.Completed)} - {step.Text}");
                }

                pollEmbed.Description = description.ToString();
                await pollMessage.ModifyAsync(null, pollEmbed.Build());
            }




            // Show the user the poll countdown
            for (int timeLeft = (int)pickTimer.TotalSeconds; timeLeft >= 0; timeLeft--)
            {
                pollEmbed.Description = $"*What game will be played next?*\nTime Left: {timeLeft}s\n\n{GameText(games, context)}";
                await pollMessage.ModifyAsync(null, pollEmbed.Build());
                await Task.Delay(1000);
            }

            // Inform the user that the winner is being computed
            pollEmbed.Description = $"Computing winner... give me a second\nGames Hosted already :{BotData.ReadData(BotVals.GAMES_HOSTED, 0)}";
            await pollMessage.ModifyAsync(null, pollEmbed.Build());

            // Get Reactions
            int[] maxIndices = await ComputeWinner(emojis, pollMessage);


            OpenVoteWinner(maxIndices,pollEmbed, pollMessage, Logger);

        }

        private async Task<int[]> ComputeWinner(DiscordEmoji[] emojis,DiscordMessage pollMessage)
        {
            var reactionTasks = emojis.Select(emoji => pollMessage.GetReactionsAsync(emoji)).ToArray();
            await Task.WhenAll(reactionTasks);

            int[] reactionCount = reactionTasks.Select(reactions => reactions.Result.Count).ToArray();

            int max = reactionCount.Max();
            Random random = new Random();
            int[] maxIndices = Enumerable.Range(0, reactionCount.Length)
                .Where(i => reactionCount[i] == max)
                .ToArray();

            return maxIndices;
        }

        private async void OpenVoteWinner(int[] maxIndices,DiscordEmbedBuilder pollEmbed, DiscordMessage pollMessage, Func<VoteStatus, Task> logger)
        {
            //Set Message Data
            pollEmbed.ImageUrl = CustomBanner.GetRandomBanner();
            pollEmbed.Title = "**Preparing your next game**";

            Random random = new Random();
            int randomIndex = maxIndices.Length == 1 ? maxIndices[0] : maxIndices[random.Next(0, maxIndices.Length)];
            PackGame GameWinner = games[randomIndex];

            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            Winner = GameWinner.Name;

            await logger(VoteStatus.OnStartingGamePack);
            await JackStreamBoxUtility.OpenGame(GameWinner.Id, logger);
            BotData.WriteData(BotVals.GAMES_HOSTED, BotData.ReadData(BotVals.GAMES_HOSTED, 0) + 1);

            ResetVote();
        }








        private string StatusEmoji(bool status)
        {
            return status ? ":thumbsup:" : ":x:";
        }


        private DiscordEmoji[] GetEmojis(CommandContext context)
        {
            
            DiscordEmoji[] emojis =
            {
                DiscordEmoji.FromName(context.Client, ":one:"),
                DiscordEmoji.FromName(context.Client, ":two:"),
                DiscordEmoji.FromName(context.Client, ":three:"),
                DiscordEmoji.FromName(context.Client, ":four:"),
                DiscordEmoji.FromName(context.Client, ":five:")
            };

            return emojis;
        }

        private string GameText(PackGame[] games, CommandContext context)
        {
            DiscordEmoji[] emojis = GetEmojis(context);
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < emojis.Length; i++)
            {
                sb.AppendLine($"{emojis[i]} {games[i].Name}\n");
            }

            return sb.ToString();
        }



        // Commands for using only numbers to vote
        [Command("1")] public async Task Vote1(CommandContext context) => await Vote(context, "1");
        [Command("2")] public async Task Vote2(CommandContext context) => await Vote(context, "2");
        [Command("3")] public async Task Vote3(CommandContext context) => await Vote(context, "3");
        [Command("4")] public async Task Vote4(CommandContext context) => await Vote(context, "4");
        [Command("5")] public async Task Vote5(CommandContext context) => await Vote(context, "5");
        [Command("6")] public async Task Vote6(CommandContext context) => await Vote(context, "6");
        [Command("7")] public async Task Vote7(CommandContext context) => await Vote(context, "7");
        [Command("8")] public async Task Vote8(CommandContext context) => await Vote(context, "8");
        [Command("9")] public async Task Vote9(CommandContext context) => await Vote(context, "9");
        [Command("10")] public async Task Vote10(CommandContext context) => await Vote(context, "10");


        //Basic explaination what vote calls a user can use
        [Command("vote")]
        public async Task vote(CommandContext context)
        {
            await context.Channel.SendMessageAsync(PackInfo.VoteCategories());
            
        }
    }
}
