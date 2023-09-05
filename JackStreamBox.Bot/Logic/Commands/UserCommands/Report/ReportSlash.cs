using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using DSharpPlus.Entities;
using DSharpPlus;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Report
{
    internal class ReportSlash : ApplicationCommandModule
    {
        [SlashCommand("report", "Report a bug of JackBot, report players to staff not here !")]
        public async Task ReportIssue(InteractionContext context, [Option("message","Report Message")] string message)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.TRUSTED)) return;
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Thx for reporting Bot Maintaner will look  into it asap!"));
            await ReportLogic.ReportIssue(context.ToCustomContext(), message);

        }
    }
}
