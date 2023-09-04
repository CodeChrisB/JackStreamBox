using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.StaffCommand.ListSettings
{
    internal class BannerCommands : BaseCommandModule
    {

        [Command("banner+")]
        [CoammandDescription("Adds a banner shown while at multiple pages", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task Add(CommandContext context, string url)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ListLogic.Add(context,ListSerializer.BANNER, url);
        }

        [Command("banner-")]
        [CoammandDescription("Removes a banner shown while at multiple pages", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]

        public async Task Remove(CommandContext context, int index)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ListLogic.Remove(context, ListSerializer.BANNER, index);
        }

        [Command("banners")]
        [CoammandDescription("Shows all banners", ":point_right:")]
        [ModCommand(PermissionRole.STAFF)]
        public async Task GetAll(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ListLogic.GetAll(context, ListSerializer.BANNER);
        }
    }
}
