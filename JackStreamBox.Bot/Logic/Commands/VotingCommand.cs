﻿using DSharpPlus;
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
        private int CURRENT_TIME = 20;
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
            ResetGameStartSteps();
        }
        private async Task LogVoteText(CommandContext context) { await context.Channel.SendMessageAsync($"Start Vote {CURRENT_VOTES.Count()}/{REQUIRED_VOTES}"); }
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


        private DiscordClient? _client;
        [Command("startvote")]
        public async Task StartVote(CommandContext context, int pack,int time)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
            await StartVoteWithPack(context, pack,time);
        }

        [Command("startvote")]
        [Description("Starts a new voting. !Closes the current game if there is one!")]
        [Requires(PermissionRole.ANYONE)]
        public async Task StartVote(CommandContext context, int pack)
        {
            await StartVoteWithPack(context, pack, TIME);
        }

        [Command("startvote")]
        public async Task StartVote(CommandContext context)
        {
            await StartVoteWithPack(context, -1,TIME);
        }

        //Vote but requires members to accept the voting process
        private async Task StartVoteWithPack(CommandContext context, int pack,int time)
        {
            int level = CommandLevel.RoleToLevel(context.Member.Roles);
            ResetGameStartSteps();

            games = pack == -1 ? PackInfo.GetRandomGames(5) : PackInfo.GetPackInfo(pack).games;


            if (level >= (int)PermissionRole.HIGHLYTRUSTED)
            {
                //Tophost and higher
                if(time>60) time = 60;
                CURRENT_TIME = time;
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
        //Vote without members to accept it
        private async Task StartVoteNow(CommandContext context, PackGame[] games)
        {
            TimeSpan span = TimeSpan.FromSeconds(CURRENT_TIME);
            
            //End Game
            JackStreamBoxUtility.CloseGame();
            //Start Vote 60 sec
            _client = context.Client;

            //----Create embed
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Game Vote",
                Description = $"Setting up the Poll"
            };
            var pollMessage = await context.Channel.SendMessageAsync(embed: pollEmbed).ConfigureAwait(false);

            //Add Reactions
            DiscordEmoji[] emojis = GetEmojis();
            foreach (var emoji in emojis)
            {
                await pollMessage.CreateReactionAsync(emoji);
            }
            //----Show votable games
            pollEmbed.Description = $"*What game will be played next?*\n(You have {CURRENT_TIME} seconds.)\n\n{GameText(games)}";
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
            var result = await interactivity.CollectReactionsAsync(pollMessage, TimeSpan.FromSeconds(CURRENT_TIME)).ConfigureAwait(false);
            var distinct = result.Distinct();
            
            Reaction[] results = result.Where(x => x.Total == result.Max(obj => obj.Total)).ToArray();
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
            
            PackGame GameWinner = ReactionToId(games, pollWinner);
            
            await pollMessage.DeleteAllReactionsAsync().ConfigureAwait(false);
            Winner = GameWinner.Name;
            
            //Reset Timer
            CURRENT_TIME = TIME;
            pollEmbed.Title = "**Preparing your next game**";
            await Logger(VoteStatus.OnStartingGamePack);
            await JackStreamBoxUtility.OpenGame(GameWinner.Id, Logger);
            Destroyer.Message(pollMessage, DestroyTime.SLOW);
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
