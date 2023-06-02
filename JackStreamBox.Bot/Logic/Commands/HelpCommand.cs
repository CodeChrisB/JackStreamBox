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
using System.Windows.Forms;

namespace JackStreamBox.Bot.Logic.Commands
{
    internal class HelpCommand : BaseCommandModule
    {
        [Command("help")]
        [Description("Explains what the bot does and how to get further help.")]
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
            sb.AppendLine("The bot is using a voting system. Members of the server will have to use **!vote** when enough people did that the bot will start the voting phase.");
            sb.AppendLine("\nAfter the voting phase the game with the most reaction will get picked and the game gets started.");
            sb.AppendLine("\n**For more information about all the diffrent commands use **\n");  
            sb.AppendLine($"**!commands+**");


            helpEmbed.Description = sb.ToString();
            var message = await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            Destroyer.Message(message, DestroyTime.REALLYSLOW);
        }

        [Command("commands")]
        [Description("List all commands you can use.")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayCommands(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, false);
            Destroyer.Message(context.Message, DestroyTime.NORMAL);
        }

        [Command("commands+")]
        [Description("Get information about every command you can use.")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayCommandsWithDescription(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            await DisplayPackWithDescription(context, true);
            Destroyer.Message(context.Message, DestroyTime.NORMAL);
        }

        public async Task DisplayPackWithDescription(CommandContext context, bool appendDescription)
        {
            
            var helpEmbed = new DiscordEmbedBuilder
            {
                Title = "Help Page™",
                Description = "If u see this message something went wrong"
            };

            CommandInfo[] ci = BotCommand.GetCommands();



            StringBuilder sb = new StringBuilder();
            int level = CommandLevel.RoleToLevel(context.Member.Roles);
            int currentlevel =-1;
            foreach (var ciItem in ci)
            {
                if(level >= (int)ciItem.Role)
                {

                    if(currentlevel < (int)ciItem.Role)
                    {
                        sb.AppendLine($"**===========╣ Level {(int)ciItem.Role} - {CommandLevel.RoleName(ciItem.Role)} ╠==========**");
                        currentlevel = (int)ciItem.Role;
                    }

                    sb.AppendLine($" **!{ciItem.Name}**");
                    if (appendDescription)
                    {
                        sb.AppendLine($"{ciItem.Description}");
                    }
                }
            }
            sb.AppendLine($"**=========╣ End of commands ╠=========**");

            helpEmbed.Description = sb.ToString();

            var pollMessage = await context.Channel.SendMessageAsync(embed: helpEmbed).ConfigureAwait(false);
            Destroyer.Message(pollMessage, DestroyTime.REALLYSLOW);
        }

        [Command("rules")]
        [Description("View the rules.")]
        [Requires(PermissionRole.ANYONE)]
        public async Task Rules(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;
            var ruleEmbed = new DiscordEmbedBuilder
            {
                Title = "Help Page",
                Description = ""
            };
            string[] rules = new string[]
            {
                "No Racism or Discrimination of Any Kind",
                "Follow the discord tos",
                "Respect for All Participants",
                "No Mid-game quitting",
                "No doxxing",
                "No spamming in game or chat",
                "Staff members may introduce other rules which will apply next to the listed rules above",
            };


            StringBuilder sb = new StringBuilder();

            sb.AppendLine("**JackStreamBox Rules**");
            int i = 1;
            foreach (var rule in rules)
            {
                sb.AppendLine($"[{i}] - {rule}");
                i++;
            }

            sb.AppendLine("\nIf you **break rules** you risk getting the @NoBot Role, which will prohibt you interacting with the bot ever again.");
            ruleEmbed.Description = sb.ToString();
            var ruleMessage = await context.Channel.SendMessageAsync(embed: ruleEmbed).ConfigureAwait(false);
            Destroyer.Message(context.Message, DestroyTime.INSTANT);
            Destroyer.Message(ruleMessage, DestroyTime.REALLYSLOW);
        }

    }
}
