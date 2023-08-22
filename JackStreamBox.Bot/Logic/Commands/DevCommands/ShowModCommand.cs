using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands._Helper.EmbedBuilder;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.DevCommands
{
    internal class ShowModCommand : BaseCommandModule
    {

        [Command("dev")]
        [Description("Show developer commands")]
        [Requires(PermissionRole.DEVELOPER)]
        public async Task Developer(CommandContext context)
        {
            CommandEmbed.Show(context, "Dev Commands", BotCommand.GetDeveloperCommands(), true);
        }

        [Command("staff")]
        [Description("Show staff commands")]
        [Requires(PermissionRole.STAFF)]
        public async Task Staff(CommandContext context)
        {
            CommandEmbed.Show(context, "Staff Commands", BotCommand.GetStaffCommands(), true);
        }
    }
}
