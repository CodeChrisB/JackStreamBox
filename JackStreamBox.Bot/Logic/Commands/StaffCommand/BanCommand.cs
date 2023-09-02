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

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand
{
    internal class BanCommand : BaseCommandModule
    {

        [Command("ban")]
        [CoammandDescription("Not working but should give a player the NoBot Role.",":skull:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task BanUser(CommandContext context, UserMention user)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;


        }
    }
}
