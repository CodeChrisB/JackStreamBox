using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Attributes;
using JackStreamBox.Bot.Logic.Commands.UserCommands.PackCommand;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Pack
{
    internal class PackCommand : BaseCommandModule
    {
        [Command("pack")]
        [CoammandDescription("View a pack and it's games. **E.g !pack 5**", ":mag:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayPack(CommandContext context, int pack)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE, true)) return;
            PackLogic.DisplayPack(context.ToCustomContext(), pack);
        }

        //DO NOT ADD REQUIRES ATTRIBUTE OTHERWISE IT WILL SHOWUP IN THE HELP COMMAND
        [Command("packs")]
        [CoammandDescription("Displays an image containing all games in all packs", ":frame_photo:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task DisplayPack(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE, true)) return;
            PackLogic.SendPackScreenshot(context.ToCustomContext());
        }
    }
}
