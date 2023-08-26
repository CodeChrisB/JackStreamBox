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
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{


    internal class SetValue : BaseCommandModule
    {

        [Command("set")]
        [Description("Set values of the bot")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Set(CommandContext context, string key, string val)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            if (!BotVals.GetKeys().Where(x => x == key).Any()) return;

            BotData.WriteData(key, val);
            var message = await context.Channel.SendMessageAsync("Done!");
            BotData.IncrementValue("message");
            Destroyer.Message(message, DestroyTime.FAST);
        }


        [Command("setview")]
        [Description("Set values of the bot")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task SetView(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;

            StringBuilder sb = new StringBuilder();


            foreach (string key in BotVals.GetKeys())
            {
                sb.AppendLine($"{key}: {BotData.ReadData(key, "!NOT SET!")}");
            }


            int size = BotData.ReadData("screen", 100);
            sb.AppendLine($"Screen Size is {16 * size}x{9 * size}");

            await context.Channel.SendMessageAsync(sb.ToString());
            BotData.IncrementValue("message");

        }
    }
}
