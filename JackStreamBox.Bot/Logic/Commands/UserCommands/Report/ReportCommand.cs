using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Report
{
    public class ReportCommand : BaseCommandModule
    {
        [Command("report")]
        [CoammandDescription("!report **Your Issue**", ":eyes:")]
        [Requires(PermissionRole.TRUSTED)]
        public async Task ReportIssue(CommandContext context, [RemainingText] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED)) return;
            await ReportLogic.ReportIssue(context.ToCustomContext(), message);
        }
    }
}
