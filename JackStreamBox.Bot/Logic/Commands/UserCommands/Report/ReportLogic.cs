using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Report
{
    internal class ReportLogic
    {
        public static async Task ReportIssue(CustomContext context, string issue)
        {
            var logChannel = await context.Client.GetChannelAsync(ChannelId.CCBBOTChannel);

            string username = context.Member.Nickname;
            await logChannel.SendMessageAsync($"{context.User.Mention} {username}:\n{issue}");
            await context.Channel.SendMessageAsync($"{context.User.Mention} thx for reporting");
        }
    }
}
8