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
            

            try
            {
                var stream = GameScreenShot.CaptureWindowScreenshot();
                if (stream == null)
                {
                    await context.Channel.SendMessageAsync("Did not work :(");
                    return;
                }

                using (MemoryStream screenshotStream = GameScreenShot.CaptureWindowScreenshot())
                {
                    // Send the screenshot using DiscordMessageBuilder

                    MemoryStream copyStream = new MemoryStream(screenshotStream.ToArray());
                    var msg = new DiscordMessageBuilder()
                        .WithContent("Screenshot")
                        .AddFile("game.png", copyStream);

                    // Send the msg using your Discord library
                    // discordClient.SendMessageAsync(channelId, msg);
                    await context.Channel.SendMessageAsync(msg);
                }



            }catch
            {
                await context.Channel.SendMessageAsync("Did not work :(");
            }

        }
    }
}
