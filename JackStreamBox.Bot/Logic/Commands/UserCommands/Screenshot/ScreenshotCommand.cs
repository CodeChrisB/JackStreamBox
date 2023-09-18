using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Attributes;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Screenshot
{
    internal class ScreenshotCommand : BaseCommandModule
    {
        //Give help when players to stupid to vote
        [Command("screenshot")]
        [CoammandDescription($"Make a  full res screenshot of the game ", ":frame_photo:")]
        [Requires(PermissionRole.ANYONE)]
        public async Task vote(CommandContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.STAFF)) return;
            await ScreenshotLogic.MakeScreenShot(context.ToCustomContext());
        }
    }
}
