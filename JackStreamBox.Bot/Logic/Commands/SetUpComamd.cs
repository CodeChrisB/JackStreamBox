using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands
{

    public class BotVals
    {
        public static readonly string VOTE_TIMER = "vote";
        public static readonly string PICK_TIMER = "pick";
        public static readonly string REQUIRED_VOTES = "require";

        public static string[] GetKeys()
        {
            return new string[] { VOTE_TIMER, PICK_TIMER, REQUIRED_VOTES };
        }

    }
    internal class SetUpComamd : BaseCommandModule
    {

        [Command("set")]
        [Description("Set values of the bot")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task Set(CommandContext context,string key, string val)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.DEVELOPER)) return;
            if (!BotVals.GetKeys().Where(x => x == key).Any()) return;

            BotSetings.WriteData<string>(key,val);
            var message = await context.Channel.SendMessageAsync("Done!");
            Destroyer.Message(message, DestroyTime.FAST);
        }

        [Command("sethelp")]
        [Description("Set values of the bot")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task SetHelp(CommandContext context, string key, string val)
        {
            await context.Channel.SendMessageAsync(
                "You can use following keys"+
                $"\nVote Timer: ${BotVals.VOTE_TIMER}"+
                $"\nPick Timer: ${BotVals.PICK_TIMER}"+
                $"\nRequired Votes ${BotVals.REQUIRED_VOTES}"
                );
        }

        [Command("setview")]
        [Description("Set values of the bot")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task SetView(CommandContext context)
        {
            var message  = await context.Channel.SendMessageAsync(
                "Current Setup Values" +
                $"\nVote Timer({BotVals.VOTE_TIMER}): {BotSetings.ReadData(BotVals.VOTE_TIMER,30)}s" +
                $"\nPick Timer({BotVals.PICK_TIMER}): {BotSetings.ReadData(BotVals.PICK_TIMER, 30)}s" +
                $"\nRequired Votes({BotVals.REQUIRED_VOTES}): {BotSetings.ReadData(BotVals.REQUIRED_VOTES, 4)}"
                );
            Destroyer.Message(message, DestroyTime.SLOW);
        }
    }
}
