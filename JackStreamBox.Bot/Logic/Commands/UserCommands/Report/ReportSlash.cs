using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Report
{
    internal class ReportSlash : ApplicationCommandModule
    {
        public async Task ReportIssue(InteractionContext context, [Choice("Report Message", 1)] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED)) return;
            await ReportLogic.ReportIssue(context.ToCustomContext(), message);
        }
    }
}
