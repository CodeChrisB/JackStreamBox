using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Help
{
    internal class HelpCommand : BaseCommandModule
    {
        [Command("help")]
        [CoammandDescription("Explains what the bot does and how to get further help.", ":question:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayHelp(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            HelpLogic.DisplayHelp(context.ToCustomContext());
        }

        [Command("commands")]
        [CoammandDescription("List all commands you can use.", ":ballot_box:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayCommands(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            HelpLogic.DisplayCommands(context.ToCustomContext());
        }
    }
}
