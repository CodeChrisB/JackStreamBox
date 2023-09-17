using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using JackStreamBox.Bot.Logic.Commands._Helper;
using JackStreamBox.Bot.Logic.Config;
using JackStreamBox.Bot.Logic.Data;
using JackStreamBox.Util;
using JackStreamBox.Util.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackStreamBox.Bot.Logic.Commands.UserCommands.Screenshot
{
    internal class ScreenshotLogic
    {
        public static async Task MakeScreenShot(CustomContext context)
        {
            if (!CommandLevel.CanExecuteCommand(context, PermissionRole.ANYONE)) return;

            try
            {
                var stream = GameScreenShot.CaptureScreenshotAsStream();
                if (stream == null)
                {
                    await context.Channel.SendMessageAsync("Did not work :(");
                    return;
                }

                var msg = new DiscordMessageBuilder()
                .WithContent("Screenshot")
                .AddFile("game.png", stream);

                await context.Channel.SendMessageAsync(msg);
            }catch
            {
                await context.Channel.SendMessageAsync("Did not work :(");
            }

        }
    }
}
