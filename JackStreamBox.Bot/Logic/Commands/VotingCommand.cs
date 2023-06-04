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

        private const int TIME = 20;
        private const int REQUIRED_VOTES = 1;
        #region Vote Declaration
        private PackGame[]? games = null;
        private struct PlayerVote
        {
            public string Player;
            public string Vote;
        }
        private List<PlayerVote> CURRENT_VOTES = new();
        private void AddVote(PlayerVote vote)
        {
            if (CURRENT_VOTES.Contains(vote)) return;
            CURRENT_VOTES.Add(vote);
        }
        public void ResetVote()
        {
            CURRENT_VOTES = new List<PlayerVote>();
            games = null;
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
                new Step { Text = BotMessage.StartingGamePack
                , Completed = false },
                new Step { Text = BotMessage.OpenedGamePack, Completed = false },
                new Step { Text = BotMessage.StartingGame, Completed = false },
                new Step { Text = BotMessage.GameOpend, Completed = false },
                new Step { Text = BotMessage.StartingStream, Completed = false },
                new Step { Text = BotMessage.AllFinished, Completed = false },
            };
        }
        #endregion


        //*******************
        //Start Voting Process
        //*******************
        [Command("vote")]
        [Description($"Vote for the pack/category you want to play, when 4 players vote one of the voted categories will be picked. ")]
        [Requires(PermissionRole.TRUSTED)]
        public async Task Vote(CommandContext context, string voteCategory)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED)) return;

            voteCategory = voteCategory.ToLower();
            switch (voteCategory)
            {
                //Packs
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                //Categories
                case "draw":
                case "trivia":
                case "talk":
                case "fun":
                    AddVote(new PlayerVote { Player = context.Member.Id.ToString(), Vote = voteCategory});
                    break;
                default:
                    await context.Channel.SendMessageAsync("use **!vote** for information what you can vote for.");
                    break;

            }


            if (CURRENT_VOTES.Count == 1)
            {
                ResetGameStartSteps();
                Task.Run(() => VoteOrCancel(context));
                await context.Channel.SendMessageAsync($"Vote now! (**!vote**)\n We need atleast {REQUIRED_VOTES} votes to start. You have 30 seconds!");

            }
        }


        public async Task VoteOrCancel(CommandContext context)
        {
            await Task.Delay(1000*30);
            Console.WriteLine("Delayed function called.");

            List<PlayerVote> votes = CURRENT_VOTES;

            if(votes.Count >= REQUIRED_VOTES)
            {
                PlayerVote vote = CURRENT_VOTES[new Random().Next(CURRENT_VOTES.Count)];
                games = PackInfo.GetVotePack(vote.Vote);
                await voteNow(context,games);
                ResetVote();
            } 
            else
            {
                await context.Channel.SendMessageAsync("Not enough votes cancel this voting.\nOnly vote after a game, you will be punished if you try to cancel a game just because you dont want to play anymore.");
            }
        }


        [Command("vote")]
        public async Task vote(CommandContext context)
        {
            await context.Channel.SendMessageAsync(PackInfo.VoteCategories());
        }

        //*******************
        //Real Voting Process
        //*******************
        private async Task voteNow(CommandContext context, PackGame[] games)
        {
            TimeSpan span = TimeSpan.FromSeconds(TIME);
            
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
            pollEmbed.Description = $"*What game will be played next?*\n(You have {TIME} seconds.)\n\n{GameText(games, context)}";
            await pollMessage.ModifyAsync(null, pollEmbed.Build());

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
            //Maybe override the function so we can use callback to resume with ALL the reactions
            var result = await interactivity.CollectReactionsAsync(pollMessage, TimeSpan.FromSeconds(TIME)).ConfigureAwait(false);
            var distinct = result.Distinct();
            
            Reaction[] results = result.Where(x => x.Total == result.Max(obj => obj.Total)).ToArray();
            //Pie Chart URL
            string pieChartUrl = PieChart.GenerateLink(result,games, GetEmojis(context));
            pollEmbed.ImageUrl = "https://user-images.githubusercontent.com/55576076/235742815-f471e12a-7e11-45ee-aad4-25b1b0aa38ab.png";
            if(!results.Any()) 
            {
                await pollMessage.DeleteAsync().ConfigureAwait(false);
                await context.Channel.SendMessageAsync("No Votes !\nCancel voting process :sob:");
                return;
            }
            //Startingphase
            //Pick Game               
            var random = new Random();
            var pollWinner = results.Length == 1 ? results[0] : results[random.Next(results.Length)];
            
            PackGame GameWinner = ReactionToId(games, pollWinner,context);
            
            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            Winner = GameWinner.Name;
            
            pollEmbed.Title = "**Preparing your next game**";
            await Logger(VoteStatus.OnStartingGamePack);
            await JackStreamBoxUtility.OpenGame(GameWinner.Id, Logger);
            Destroyer.Message(pollMessage, DestroyTime.SLOW);
        }
        private PackGame ReactionToId(PackGame[] games, Reaction pollWinner, CommandContext context)
        {
            int index = 0;
            DiscordEmoji[] emojis = GetEmojis(context);

            for(int i = 0; i < emojis.Length;i++)
            {
                if (emojis[i].Name == pollWinner.Emoji.Name) index = i;
            }

            return games[index];
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
    }
}
