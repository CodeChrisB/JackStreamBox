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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Voting
{
    internal class VoteLogic
    {
        #region Vote Declaration

        //Vote Props
        private static List<string> voteCategories = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", /*"draw", "trivia", "talk", "fun"*/ };


        private static Dictionary<string, string> PackVotes = new Dictionary<string, string>();
        private static Dictionary<ulong, int> GameVotes = new Dictionary<ulong, int>();
        private static PackGame[]? games = null;
        private static int 
            
            timeTillVoteEnd = 0;
        private static bool currentlyVoting = false;

        private static DateTime lockOutTill = new DateTime();
        public static bool IsLocked() { return DateTime.Now<lockOutTill; }



        //Discord Props
        private static DiscordMessage PackVoteMessage;
        private static DiscordMessage GameVoteMessage;
        private static DiscordEmbedBuilder PrePollMessageData = new DiscordEmbedBuilder { };
        private static DiscordEmbedBuilder GameVoteData = new DiscordEmbedBuilder { };



        public static void ResetVote()
        {
            PackVotes = new Dictionary<string, string>();
            GameVotes = new Dictionary<ulong, int>();
            games = null;
            currentlyVoting = false;
            PackVoteMessage = null;
            GameVoteMessage = null;
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
                if(voteCategory == "10")
                {
                    await ccontext.Channel.SendMessageAsync("I have pack 10 installed but I can't stream it til release");

                }
                else
                {
                    await ccontext.Channel.SendMessageAsync("use **!vote** for information what you can vote for.");
                }
                return;
            }



            //Valid Set Vote

            PackVotes[id>0 ? id.ToString() : ccontext.Member.Id.ToString()] = voteCategory;

            
            if (PackVotes.Count == 1 && PackVoteMessage == null)
            {
                ResetGameStartSteps();
                OnPackVoteEnd(ccontext);
                

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
        public static async void VoteViaMenu(CustomContext customContext, string id,DateTime menuCreationTime)
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
                case ButtonId.PACK10: packId = "10"; break;
            }


            Vote(customContext, packId,customContext.User.Id);
        }

        public static void OnGameVote(CustomContext context, string id )
        {
            int gameId = 0;
            switch (id)
            {
                case ButtonId.VOTE1: gameId = 0; break;
                case ButtonId.VOTE2: gameId = 1; break;
                case ButtonId.VOTE3: gameId = 2; break;
                case ButtonId.VOTE4: gameId = 3; break;
                case ButtonId.VOTE5: gameId = 4; break;

            }
            GameVotes[context.User.Id] = gameId;
        }
        //Helpers for the Voting
        private static void ModifyPackVoteMessage()
        {
            if (PackVoteMessage == null) return;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Time Left: {timeTillVoteEnd}s");

            int requiredVotes = BotData.ReadData(BotVals.REQUIRED_VOTES, 4);

            if (PackVotes.Values.Count < requiredVotes)
                sb.AppendLine($"Required Votes: {PackVotes.Values.Count}/{requiredVotes} :x:");
            else
                sb.AppendLine($"Required Votes: :white_check_mark:");


            foreach (var key in voteCategories)
                sb.AppendLine($"**!{key}** : {PackVotes.Count(x => x.Value == key)}");

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

        private static void ModifyGameVoteMessage(int timeLeft,CustomContext context)
        {
            if (GameVoteMessage == null) return;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*What game will be played next?*");
            sb.AppendLine($"Time Left: {timeLeft}s");
            sb.AppendLine(GameText(games,context));

            char winner = '█';
            char loser = '▒';
            int maxVotes = CurrentGameVotes.Max();
            for (int i = 0; i < games.Length; i++)
            {
                char filling = maxVotes == CurrentGameVotes[i] ? winner : loser;
                sb.AppendLine($"({i + 1}) | {new string(filling, CurrentGameVotes[i]*2+1)}");
            }
            /*
            5 |███████████████████         (17)
            4 |██████████████████████████   (19)
            3 |████████████████████████████ (20)
            2 |█████████████████████         (15)
            1 |███████████████               (11)
                0   5   10  15  20
             
             
             */

            GameVoteData.Description = sb.ToString() ;
            GameVoteMessage.ModifyAsync(GameVoteData.Build());
        }

        private static async void OnPackVoteEnd(CustomContext context)
        {
            timeTillVoteEnd = BotData.ReadData(BotVals.VOTE_TIMER, 30);

            while (timeTillVoteEnd >= 0)
            {
                await Task.Delay(1000);
                ModifyPackVoteMessage();
                timeTillVoteEnd--;
            }


            List<string> votes = PackVotes.Values.ToList();

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
                PlainEmbed
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
            GameVoteData = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = "Setting up the Poll"
            };
            GameVoteMessage = await context.Channel.SendMessageAsync(embed: GameVoteData).ConfigureAwait(false);
            var gameVoteMessage = await GameVoteMenu(context);





            // Show the user the poll countdown
            for (int timeLeft = (int)pickTimer.TotalSeconds; timeLeft >= 0; timeLeft--)
            {
                ModifyGameVoteMessage(timeLeft,context);
                await Task.Delay(1000);
            }






   

            Destroyer.Message(gameVoteMessage, DestroyTime.INSTANT);
            OpenVoteWinner();

        }

        private static int[] CurrentGameVotes
        {
            get
            {
                int[] games = new int[5];
                foreach (int game in GameVotes.Values.ToArray())
                {
                    games[game]++;
                }
                return games;
            }
        }
        
        private static int[] WinnerIndices
        {
            get
            {
                int max = CurrentGameVotes.Max();
                int[] maxIndices = Enumerable.Range(0, CurrentGameVotes.Length)
                .Where(i => CurrentGameVotes[i] == max)
                .ToArray();

                return maxIndices;
            }
        }

        private static async Task<DiscordMessage> GameVoteMenu(CustomContext context)
        {
            DiscordButtonComponent Btn(string id, string emoji)
            {
                return new DiscordButtonComponent(ButtonStyle.Secondary, id, "", false, new DiscordComponentEmoji(emoji));
            }
            var builder = new DiscordMessageBuilder()
                .WithContent("-----")
                .AddComponents(new DiscordComponent[]
                {
                    Btn(ButtonId.VOTE1,"1️⃣"),
                    Btn(ButtonId.VOTE2,"2️⃣"),
                    Btn(ButtonId.VOTE3,"3️⃣"),
                    Btn(ButtonId.VOTE4,"4️⃣"),
                    Btn(ButtonId.VOTE5,"5️⃣")
                });

            return await context.Channel.SendMessageAsync(builder);

        }

        //Helpers for getting Winner & Start Game

        private static async void OpenVoteWinner()
        {
            //Set Message Data
            GameVoteData.ImageUrl = ListSerializer.GetRandomEntry(ListSerializer.BANNER);
            GameVoteData.Title = "**Preparing your next game**";

            Random random = new Random();
            int randomIndex = WinnerIndices.Length == 1 ? 
                WinnerIndices[0] : 
                WinnerIndices[random.Next(0, WinnerIndices.Length)];

            PackGame GameWinner = games[randomIndex];
            Winner = GameWinner.Name;

            // Logger Setup Phase
            async Task Logger(VoteStatus status)
            {
                GameStartSteps[(int)status].Completed = true;

                var description = new StringBuilder($"Winner is {Winner}\n");

                foreach (var step in GameStartSteps)
                {
                    description.AppendLine($"{StatusEmoji(step.Completed)} - {step.Text}");
                }


                GameVoteData.Description = description.ToString();
                await GameVoteMessage.ModifyAsync(null, GameVoteData.Build());
            }

            await Logger(VoteStatus.OnStartingGamePack);
            await JackStreamBoxUtility.OpenGame(GameWinner.Id, Logger);
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

        private static bool IsOlderThanNSeconds(DateTime dateTimeToCheck, int seconds)
        {
            // Get the current DateTime
            DateTime currentDateTime = DateTime.Now;

            // Calculate the TimeSpan between the current DateTime and the dateTimeToCheck
            TimeSpan timeDifference = currentDateTime - dateTimeToCheck;

            // Check if the timeDifference is greater than or equal to "n" seconds
            return timeDifference.TotalSeconds >= seconds;
        }
    }
}
