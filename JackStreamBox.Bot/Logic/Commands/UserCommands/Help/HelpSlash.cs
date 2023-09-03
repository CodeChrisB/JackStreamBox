using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Data;
using DSharpPlus.SlashCommands;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Help
{
    internal class HelpSlash : ApplicationCommandModule
    {
        [SlashCommand("help","Gives information about using the bot")]
        public async Task DisplayHelp(InteractionContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            HelpLogic.DisplayHelp(context.ToCustomContext());
        }

        [SlashCommand("commands", "List all commands you can use.")]
        public async Task DisplayCommands(InteractionContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            HelpLogic.DisplayCommands(context.ToCustomContext());
        }

        [SlashCommand("rules","View all rules, dont break them u get punished. We mean it!")]
        public async Task Rules(InteractionContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            HelpLogic.ShowRules(context.ToCustomContext());
        }
    }
}
