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
    internal class HelpCommand : BaseCommandModule
    {

        [Command("help")]
        [Description("Use this command to get information about every command you can use")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayPack(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, false);
        }

        [Command("extrahelp")]
        [Description("Use this command to get information about every command you can use")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayPack2(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, true);
        }


        public async Task DisplayPackWithDescription(CommandContext context, bool appendDescription)
        {
            
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = "Help Page",
                Description = $"Todo add info"
            };

            CommandInfo[] ci = BotCommand.GetCommands();

            StringBuilder sb = new StringBuilder();

            foreach (var ciItem in ci)
            {
                sb.AppendLine($"!{ciItem.Name}  - [Level {(int)ciItem.Role}]");
                if(appendDescription)
                {
                    sb.AppendLine(ciItem.Description);
                }
            }

            helpEmbed.Description = sb.ToString();

            var pollMessage = await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
        }

    }
}
