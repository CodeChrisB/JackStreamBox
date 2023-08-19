using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
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
using System.Windows;
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class VotingCommand : BaseCommandModule
    {
        private int timeTillVoteEnd =0;
        #region Vote Declaration
        private PackGame[]? games = null;

        private List<string> voteCategories = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", /*"draw", "trivia", "talk", "fun"*/ };

        Dictionary<string, string> VotesOfPlayers = new Dictionary<string, string>();

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
        [Description($"Vote for the pack/category you want to play, when 4 players vote one of the voted categories will be picked. ")]
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

            
            foreach( var key in voteCategories )
            {
                sb.AppendLine($"**!vote {key}** : {VotesOfPlayers.Count(x => x.Value == key)}");
            }

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



            Console.WriteLine("Delayed function called.");

            List<string> votes = VotesOfPlayers.Values.ToList();

            if(votes.Count >= BotData.ReadData(BotVals.REQUIRED_VOTES, 3))
            {
                string vote = votes[new Random().Next(votes.Count)];
                games = PackInfo.GetVotePack(vote);
                await voteNow(context,games);
                ResetVote();
            } 
            else
            {
                await context.Channel.SendMessageAsync("Not enough votes cancel this voting.\nOnly vote after a game, you will be punished if you try to cancel a game just because you dont want to play anymore.");
            }
        }

        private async Task voteNow(CommandContext context, PackGame[] games)
        {
            TimeSpan span = TimeSpan.FromSeconds(BotData.ReadData(BotVals.VOTE_TIMER,30));
            
            //End Game
            JackStreamBoxUtility.CloseGame();
            //Start Vote 60 sec

            //----Create embed
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = $"Setting up the Poll"
            };
            var pollMessage = await context.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            //Add Reactions
            DiscordEmoji[] emojis = GetEmojis(context);
            foreach (var emoji in emojis)
            {
                await pollMessage.CreateReactionAsync(emoji);
            }
            //----Show votable games


            //----Voting Phase
            async Task Logger(VoteStatus status)
            {
                GameStartSteps[(int)status].Completed = true;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"Winner is {Winner}\n");
                foreach(Step step in GameStartSteps)
                {
                    sb.AppendLine($"{StatusEmoji(step.Completed)} - {step.Text}");
                }

                pollEmbed.Description = sb.ToString();
                await pollMessage.ModifyAsync(null, pollEmbed.Build());
            }
            string StatusEmoji(bool status)
            {
                if (status) return ":thumbsup:";
                return ":x:";
            }


            //Get Reactions
            var interactivity = context.Client.GetInteractivity();
            var result = interactivity.CollectReactionsAsync(pollMessage,TimeSpan.FromSeconds(BotData.ReadData(BotVals.VOTE_TIMER, 30)));

            //Show the user the poll
            int timeLeft = BotData.ReadData(BotVals.PICK_TIMER, 30);
            while (timeLeft >= 0)
            {
                pollEmbed.Description = $"*What game will be played next?*\nTime Left: {timeLeft}s\n\n{GameText(games, context)}";
                await pollMessage.ModifyAsync(null, pollEmbed.Build());
                timeLeft--;
                await Task.Delay(1000);
            }

            //Tellin user we compute the winner
            pollEmbed.Description = $"Computing winner... give me a second\nGames Hosted already :{BotData.ReadData(BotVals.GAMES_HOSTED, "0")}";
            await pollMessage.ModifyAsync(null, pollEmbed.Build());



            //Get Reactions 
            IReadOnlyList<DiscordUser>[] reactions = new IReadOnlyList<DiscordUser>[] {
                await pollMessage.GetReactionsAsync(GetEmojis(context)[0]),
                await pollMessage.GetReactionsAsync(GetEmojis(context)[1]),
                await pollMessage.GetReactionsAsync(GetEmojis(context)[2]),
                await pollMessage.GetReactionsAsync(GetEmojis(context)[3]),
                await pollMessage.GetReactionsAsync(GetEmojis(context)[4])
            };

            int[] reactionCount = new int[]
            {
                reactions[0].Count,
                reactions[1].Count,
                reactions[2].Count,
                reactions[3].Count,
                reactions[4].Count,
            };

            int max = reactionCount.Max();
            Random random = new Random();
            int[] maxIndices = reactionCount.Select((n, i) => (Number: n, Index: i))
                                      .Where(pair => pair.Number == max)
                                      .Select(pair => pair.Index)
                                      .ToArray();

            int randomIndex = maxIndices.Length == 1 ? maxIndices[0] : maxIndices[random.Next(0, maxIndices.Length)];
            pollEmbed.ImageUrl = "https://media.discordapp.net/attachments/1066085138791932005/1135296119610552350/7u88ip.png";

        

            PackGame GameWinner = games[randomIndex];
            
            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            Winner = GameWinner.Name;
            
            pollEmbed.Title = "**Preparing your next game**";
            await Logger(VoteStatus.OnStartingGamePack);
            await JackStreamBoxUtility.OpenGame(GameWinner.Id, Logger);

            ResetVote();
            Destroyer.Message(pollMessage, DestroyTime.SLOW);
            BotData.AddGameHost();
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


        //Basic explaination what vote calls a user can use
        [Command("vote")]
        public async Task vote(CommandContext context)
        {
            await context.Channel.SendMessageAsync(PackInfo.VoteCategories());
        }
    }
}
