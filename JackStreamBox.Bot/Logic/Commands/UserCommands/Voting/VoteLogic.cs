using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Voting
{
    internal class VoteLogic
    {
        #region Vote Declaration

        //Vote Props
        private static List<string> voteCategories = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", /*"draw", "trivia", "talk", "fun"*/ };
        private static Dictionary<string, string> VotesOfPlayers = new Dictionary<string, string>();
        private static PackGame[]? games = null;
        private static int timeTillVoteEnd = 0;
        private static bool currentlyVoting = false;

        private static DateTime lockOutTill = new DateTime();
        public static bool IsLocked() { return DateTime.Now<lockOutTill; }



        //Discord Props
        private static DiscordMessage PackVoteMessage;
        private static DiscordEmbedBuilder PrePollMessageData = new DiscordEmbedBuilder { };



        public static void ResetVote()
        {
            VotesOfPlayers = new Dictionary<string, string>();
            games = null;
            currentlyVoting = false;
            PackVoteMessage = null;
            PrePollMessageData = null;
            ResetGameStartSteps();
        }
        #endregion

        #region Step
        public struct Step
        {
            public string Text;
            public bool Completed;
        }

        private static Step[] GameStartSteps = new Step[1];
        private static string Winner = "";

        private static void ResetGameStartSteps()
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


        //Methods for Player Voting
        public static async void Vote(CustomContext ccontext, string voteCategory,ulong id = 0)
        {
            if (DateTime.Now < lockOutTill)
            {
                ResetVote();
                SendLockOutMessage(ccontext);
                return;
            }


            //Not Valid? Send Message and Stop
            if (!IsValidCategory(voteCategory))
            {
                await ccontext.Channel.SendMessageAsync("use **!vote** for information what you can vote for.");
                return;
            }


            //Valid Set Vote

            VotesOfPlayers[id>0 ? id.ToString() : ccontext.Member.Id.ToString()] = voteCategory;

            
            if (VotesOfPlayers.Count == 1 && PackVoteMessage == null)
            {
                ResetGameStartSteps();
                Task.Run(() => OnPackVoteEnd(ccontext));
                

                PrePollMessageData = PlainEmbed
                    .CreateEmbed(ccontext)
                    .Title("Game Vote")
                    .Description("Setting up the Poll")
                    .Color(DiscordColor.Green)
                    .GetBuilder();

                PackVoteMessage = await ccontext.Channel.SendMessageAsync(embed: PrePollMessageData).ConfigureAwait(false);

            }
            ModifyPackVoteMessage();
        }
        public static async Task VoteViaSlash(InteractionContext context, string voteCategory)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            Vote(context.ToCustomContext(), voteCategory);
            await context
                .CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("Done!"));
        }
        public static async void VoteViaMenu(CustomContext customContext, string id)
        {
            string packId = "";
            switch (id)
            {
                case ButtonId.PACK1: packId = "1"; break;
                case ButtonId.PACK2: packId = "2"; break;
                case ButtonId.PACK3: packId = "3"; break;
                case ButtonId.PACK4: packId = "4"; break;
                case ButtonId.PACK5: packId = "5"; break;
                case ButtonId.PACK6: packId = "6"; break;
                case ButtonId.PACK7: packId = "7"; break;
                case ButtonId.PACK8: packId = "8"; break;
                case ButtonId.PACK9: packId = "9"; break;
                case ButtonId.PACK10: packId = "9"; break;
            }


            Vote(customContext, packId,customContext.User.Id);
        }
        //Helpers for the Voting
        private static void ModifyPackVoteMessage()
        {
            if (PackVoteMessage == null) return;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Time Left: {timeTillVoteEnd}s");

            int requiredVotes = BotData.ReadData(BotVals.REQUIRED_VOTES, 4);

            if (VotesOfPlayers.Values.Count < requiredVotes)
                sb.AppendLine($"Required Votes: {VotesOfPlayers.Values.Count}/{requiredVotes} :x:");
            else
                sb.AppendLine($"Required Votes: :white_check_mark:");


            foreach (var key in voteCategories)
                sb.AppendLine($"**!{key}** : {VotesOfPlayers.Count(x => x.Value == key)}");

            if (timeTillVoteEnd > 0)
            {
                PrePollMessageData.Description = sb.ToString();
                PackVoteMessage.ModifyAsync(PrePollMessageData.Build());
            }
            else
            {
                if (PackVoteMessage == null) return;
                PackVoteMessage.DeleteAsync();
            }

        }
        private static async Task OnPackVoteEnd(CustomContext context)
        {
            timeTillVoteEnd = BotData.ReadData(BotVals.VOTE_TIMER, 30);

            while (timeTillVoteEnd >= 0)
            {
                await Task.Delay(1000);
                ModifyPackVoteMessage();
                timeTillVoteEnd--;
            }


            List<string> votes = VotesOfPlayers.Values.ToList();

            if (votes.Count >= BotData.ReadData(BotVals.REQUIRED_VOTES, 3))
            {
                string vote = votes[new Random().Next(votes.Count)];
                games = PackInfo.GetVotePack(vote);
                SetTimeout();
                await VoteNow(context, games);
            }
            else
            {
                ResetTimeout();
                await PlainEmbed
                    .CreateEmbed(context)
                    .Title("Not Enough Votes !")
                    .DescriptionAddLine($"Yikes... we need some more votes to start a game\n")
                    .DescriptionAddLine($"Next time try it with atleast {BotData.ReadData(BotVals.REQUIRED_VOTES, 3)} votes")
                    .DescriptionAddLine("Just trying to start a new vote to end the current game will revoke your bot rights.")
                    .BuildNDestroy(DestroyTime.SLOW);
            }
            ResetVote();
        }

        //Game Voter
        private static async Task VoteNow(CustomContext context, PackGame[] games)
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


            OpenVoteWinner(maxIndices, pollEmbed, pollMessage, Logger);

        }

        //Helpers for getting Winner & Start Game

        private static async Task<int[]> ComputeWinner(DiscordEmoji[] emojis, DiscordMessage pollMessage)
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

        private static async void OpenVoteWinner(int[] maxIndices, DiscordEmbedBuilder pollEmbed, DiscordMessage pollMessage, Func<VoteStatus, Task> logger)
        {
            //Set Message Data
            pollEmbed.ImageUrl = ListSerializer.GetRandomEntry(ListSerializer.BANNER);
            pollEmbed.Title = "**Preparing your next game**";

            Random random = new Random();
            int randomIndex = maxIndices.Length == 1 ? maxIndices[0] : maxIndices[random.Next(0, maxIndices.Length)];
            PackGame GameWinner = games[randomIndex];

            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            Winner = GameWinner.Name;

            await logger(VoteStatus.OnStartingGamePack);
            await JackStreamBoxUtility.OpenGame(GameWinner.Id, logger);
            BotData.WriteData(BotVals.GAMES_HOSTED, (BotData.ReadData(BotVals.GAMES_HOSTED, 0) + 1).ToString());

            ResetVote();
        }

        
        //Actual helpers 
        private static bool IsValidCategory(string voteCategory)
        {
            voteCategory = voteCategory.ToLower();

            if (voteCategories.IndexOf(voteCategory) > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string StatusEmoji(bool status)
        {
            return status ? ":thumbsup:" : ":x:";
        }

        private static DiscordEmoji[] GetEmojis(CustomContext context)
        {
            return new DiscordEmoji[] {
                DiscordEmoji.FromName(context.Client, ":one:"),
                DiscordEmoji.FromName(context.Client, ":two:"),
                DiscordEmoji.FromName(context.Client, ":three:"),
                DiscordEmoji.FromName(context.Client, ":four:"),
                DiscordEmoji.FromName(context.Client, ":five:")
            };
        }

        private static string GameText(PackGame[] games, CustomContext context)
        {
            DiscordEmoji[] emojis = GetEmojis(context);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < emojis.Length; i++)
            {
                sb.AppendLine($"{emojis[i]} {games[i].Name}\n");
            }

            return sb.ToString();
        }


        public static void SetTimeout()
        {
            // Get the current datetime
            DateTime currentDateTime = DateTime.Now;

            // Add the TimeSpan to the current datetime
            DateTime resultDateTime = currentDateTime.Add(TimeSpan.FromSeconds(BotData.ReadData(BotVals.VOTE_TIMEOUT, 300)));

            lockOutTill = resultDateTime;
        }

        public static void ResetTimeout()
        {
            lockOutTill = DateTime.Now;    
        }

        public static async void SendLockOutMessage(CustomContext context)
        {
            TimeSpan timeUntilTarget = lockOutTill - DateTime.Now;
            await context.Channel.SendMessageAsync($"We just voted! Next vote can be started in {(int)timeUntilTarget.TotalMinutes} Minutes and {(int)(timeUntilTarget.TotalSeconds % 60)} seconds.");
        }
    }
}
