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
using JackStreamBox.Util.Data;
using System.Windows.Input;

namespace JackStreamBox.Bot.Logic.Commands
{


    internal class SetUpComamd : BaseCommandModule
    {

        [Command("set")]
        [Description("Set values of the bot")]
        [Requires(PermissionRole.STAFF)]
        public async Task Set(CommandContext context,string key, string val)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            if (!BotVals.GetKeys().Where(x => x == key).Any()) return;

            BotData.WriteData<string>(key,val);
            var message = await context.Channel.SendMessageAsync("Done!");
            Destroyer.Message(message, DestroyTime.FAST);
        }

        [Command("sethelp")]
        [Description("Set values of the bot")]
        [Requires(PermissionRole.STAFF)]
        public async Task SetHelp(CommandContext context, string key, string val)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            if (!BotVals.GetKeys().Where(x => x == key).Any()) return;

            await context.Channel.SendMessageAsync(
                "You can use following keys"+
                $"\nVote Timer: ${BotVals.VOTE_TIMER}"+
                $"\nPick Timer: ${BotVals.PICK_TIMER}"+
                $"\nRequired Votes ${BotVals.REQUIRED_VOTES}"+
                $"\nBot Name ${BotVals.BOT_NAME}"
                );
        }

        [Command("setview")]
        [Description("Set values of the bot")]
        [Requires(PermissionRole.STAFF)]
        public async Task SetView(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;

            StringBuilder sb = new StringBuilder();


            foreach (string key in BotVals.GetKeys())
            {
                sb.AppendLine($"{key}: {BotData.ReadData(key, "!NOT SET!")}");
            }

            await context.Channel.SendMessageAsync(sb.ToString());
            
        }
    }
}
