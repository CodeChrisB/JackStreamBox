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
        [Description("List all commands you can use")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayHelp(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = "**How to use the bot?**",
                Description = ""
            };

            
            StringBuilder sb = new StringBuilder();                                     
            sb.AppendLine("");
            sb.AppendLine("The bot is using a voting system. Members of the server will have to use **!startvote** when enough people did that the bot will start the voting phase.");
            sb.AppendLine("\nAfter the voting phase the game with the most reaction will get picked and the game gets started.");
            sb.AppendLine("\n");
            sb.AppendLine("For more information about all the diffrent commands use **\n");  
            sb.AppendLine($"**!commands+**");


            helpEmbed.Description = sb.ToString();
            await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
        }

        [Command("commands")]
        [Description("List all commands you can use")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayCommands(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, false);
        }

        [Command("commands+")]
        [Description("Get information about every command you can use")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayCommandsWithDescription(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, true);
        }


        public async Task DisplayPackWithDescription(CommandContext context, bool appendDescription)
        {
            
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = "Help Page",
                Description = "If u see this message something went wrong"
            };

            CommandInfo[] ci = BotCommand.GetCommands();

            StringBuilder sb = new StringBuilder();
            int level = CommandLevel.RoleToLevel(context.Member.Roles);
            foreach (var ciItem in ci)
            {
                if(level >= (int)ciItem.Role)
                {
                    sb.AppendLine($"**!{ciItem.Name}**  - [Level {(int)ciItem.Role}]");
                    if (appendDescription)
                    {
                        sb.AppendLine($"{ciItem.Description}\n");
                    }
                }
            }

            helpEmbed.Description = sb.ToString();

            var pollMessage = await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
        }

    }
}
