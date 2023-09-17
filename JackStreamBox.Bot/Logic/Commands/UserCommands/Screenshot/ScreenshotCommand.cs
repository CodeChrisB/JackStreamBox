using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext;
using JackStreamBox.Bot.Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JackStreamBox.Bot.Logic.Config.ExtensionMethods;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Screenshot
{
    internal class ScreenshotCommand : BaseCommandModule
    {
        //Give help when players to stupid to vote
        [Command("screenshot")]
        public async Task vote(CommandContext context)
        {
            await ScreenshotLogic.MakeScreenShot(context.ToCustomContext());
        }
    }
}
