using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
    public class ReportCommand : BaseCommandModule
    {
        [Command("report")]
        [Description("!report **\"**Your Issue**\"")]
        [Requires(PermissionRole.TRUSTED)]
        public async Task ReportIssue(CommandContext context,string issue)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED)) return;

            var logChannel = await context.Client.GetChannelAsync(1114225698056445992);

            string username = context.Member.Nickname;
            await logChannel.SendMessageAsync($"{username}\n{issue}");

        }
    }
}
